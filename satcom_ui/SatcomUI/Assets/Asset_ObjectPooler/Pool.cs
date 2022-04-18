using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/**
 *  Pool
 *  desc:
 *  manages a specific set of objects of the same type and their pooling.
 **/
public class Pool : MonoBehaviour {

    //objects that are currently not in use and ready to be used
    private Queue<GameObject> readyObjects = new Queue<GameObject>(0);
    //objects that are currently in use and will only be used via recycling
    private Queue<GameObject> inUseObjects = new Queue<GameObject>(0);
    private int totalObjectCount
    {
        get
        {
            return readyObjects.Count + inUseObjects.Count;
        }
    }
    public PoolManager PoolManager {
        get;
        set;
    }
    [Tooltip("The id which this pool is known by")]
    [SerializeField]
    private string id;
    public string Id
    {
        get
        {
            return id;
        }
    }
    [Tooltip("The source prefab for this pool")]
    [SerializeField]
    private GameObject sourcePrefab;
    public GameObject SourcePrefab
    {
        get
        {
            return sourcePrefab;
        }
    }
    [Tooltip("The amount of objects to be initilized at start")]
    [SerializeField]
    private int initialObjects;
    [Tooltip("The max amount of objects allowed in this pool")]
    [SerializeField]
    private int maxObjects = 10;
    [Tooltip("There is no max amount of objects")]
    [SerializeField]
    private bool noLimit; //true = there is no max amount, new objects will be added when needed
    public bool NoLimit
    {
        get
        {
            return noLimit;
        }
    }
    [Tooltip("This pool will recycle objects when all objects are in use")]
    [SerializeField]
    private bool recycleObjects = true; //should objects be recyled once max objects is reached, otherwise an error is thrown
    [Tooltip("This pool will recycle visible objects (MUST have a mesh renderer attached if false)")]
    [SerializeField]
    private bool recycleVisible = true; //should this pool recycle visible objects
    [Tooltip("Should this pool have a parent object for all the objects in the pool")]
    [SerializeField]
    private bool havePoolParent;
    public bool HavePoolParent
    {
        get
        {
            return havePoolParent;
        }
    }
    [Tooltip("the object that this pool will set all objects in the pool as a parent to when they are pooled")]
    [SerializeField]
    private GameObject poolParent;
    public GameObject PoolParent
    {
        get
        {
            return poolParent;
        }
    }
    /*
     * Awake
     * desc: MonoBehavior.Awake()
     */
    private void Awake()
    {
        //fill the queue with initial objects
        for (int i = 0; i < initialObjects; i++)
        {
            CreateObject();
        }
        //making sure some parameters are within valid ranges
        maxObjects = maxObjects < 1 ? 1 : maxObjects;
    }
    /*
     * CreateObject
     * desc: creates a new object of this pools type and adds it to the ready queue
     */
    private void CreateObject()
    {
        
        GameObject newObject = Instantiate(sourcePrefab);
        if (havePoolParent)
        {
            newObject.transform.SetParent(poolParent.transform);
        }
        newObject.SetActive(false);
        readyObjects.Enqueue(newObject);
    }
    /*
     * PoolObject
     * time: O(1)
     * desc: get an object from this pool
     */
    public GameObject PoolObject ()
    {
        if (readyObjects.Count > 0) //checks if ready objects has objects to use
        {
            GameObject newObject = readyObjects.Dequeue();
            inUseObjects.Enqueue(newObject);
            return newObject;
        }
        if (totalObjectCount < maxObjects) //checks if we should increase pool size
        {
            CreateObject();
            GameObject newObject = readyObjects.Dequeue();
            inUseObjects.Enqueue(newObject);
            return newObject;
        }
        if (recycleObjects) //checks if we should recycle object
        {
            GameObject newObject = inUseObjects.Dequeue();
            if (!recycleVisible) //checks if we can't recycle visible objects
            {
                //checks visibility on the object
                Renderer rend = newObject.GetComponentInChildren<Renderer>();
                if (rend == null) rend = newObject.GetComponent<Renderer>();
                if (rend == null)
                {
                    Errors.LogError("NoRenderer", newObject.name);
                    return null;
                }
                if (rend.isVisible)
                {
                    //try again on another object in the pool
                    inUseObjects.Enqueue(newObject);
                    try
                    {
                        return PoolObject();
                    } catch (IOException ioe)
                    {
                        Errors.LogError("IOError", ioe.Message);
                    }
                }
            }
            //since this object is being depooled and repooled, we must call depool is needed
            if (newObject.GetComponent<IPoolable>() != null)
            {
                newObject.GetComponent<IPoolable>().OnDePool();
            }
            inUseObjects.Enqueue(newObject);
            return newObject;
        }
        Errors.LogError("OutOfObjects");
        return null;
    }
    /*
     * DePoolObject
     * desc: will make sure [item] is ready to be pooled again
     * time: O(n)
     * parameters: item (the object to put back into the readyObjects queue)
     */
    public void DePoolObject (GameObject item)
    {
        if (readyObjects.Contains(item))
        {
            return;
        }
        Queue<GameObject> newQueue = new Queue<GameObject>(0);
        //go through inUseQueue, checking for the object, if found exit
        while (inUseObjects.Count > 0)
        {
            GameObject newObject = inUseObjects.Dequeue();
            if (newObject == item)
            {
                OnDepool(newObject);
            }
            newQueue.Enqueue(newObject);
        }
        inUseObjects = newQueue;
    }
    /*
     * DePoolAll()
     * desc: depools all the objects in this pool
     */
    public void DePoolAll ()
    {
        //go through the in use queue disabling objects
        while (inUseObjects.Count > 0) 
        {
            GameObject newObject = inUseObjects.Dequeue();
            OnDepool(newObject);
        }
    }
    /*
     * OnDepool
     * desc: a helper method for depooling an object
     */
    private void OnDepool(GameObject newObject)
    {
        readyObjects.Enqueue(newObject);
        newObject.SetActive(false);
        //on depool
        if (newObject.GetComponent<IPoolable>() != null)
        {
            newObject.GetComponent<IPoolable>().OnDePool();
        }
    }
}
