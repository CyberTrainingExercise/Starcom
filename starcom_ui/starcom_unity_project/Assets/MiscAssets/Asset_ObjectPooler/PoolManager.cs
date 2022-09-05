using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PoolUIType
 * desc: enum used in displaying pool managers in either full or compactView
 */
public enum PoolUIType { fullView, compactView};

/**
 *  PoolManager
 *  desc:
 *  manages a set of [Pool]s and all their associated objects.
 *  will add itself to the ObjectPooler to be able to be easily used by the project creator.
 **/
public class PoolManager : MonoBehaviour {

    [SerializeField]
    private string poolManagerID = "main";

    private Hashtable pools = new Hashtable(); //hashtable of pools, key is pool.id

    [SerializeField]
    public PoolUIType poolUIType = PoolUIType.fullView;

    /*
     * Awake
     * desc: MonoBehavior.Awake()
     */
    private void Awake()
    {
        GatherPools();
        ObjectPooler.InitilizePool(this, poolManagerID);
    }
    /*
     * GatherPools
     * desc: gathers all the pool objects on this object and puts them into the hash table
     */
    private void GatherPools ()
    {
        Pool[] poolArray = GetComponents<Pool>();
        foreach (Pool pool in poolArray)
        {
            EnterPool(pool);
        }
    }
    /*
     * EnterPool
     * desc: adds a pool to the hash table
     */
    private void EnterPool (Pool pool)
    {
        pools.Add(pool.Id, pool);
    }
    /*
     *  PoolObject
     *  desc: pools an object of type [id]
     *  paramters: id (the type of object you wish to pool)
     */
    public GameObject PoolObject (string id)
    {
        //checks if the object id exists
        if (!pools.ContainsKey(id))
        {
            Errors.LogError("PoolNullDefinition", id);
            return null;
        }
        //pools an object of id [id]
        Pool pool = (Pool)pools[id];
        GameObject returnObject = pool.PoolObject();
        if (returnObject != null) returnObject.SetActive(true);
        //on pool
        if (returnObject.GetComponent<IPoolable>() != null)
        {
            returnObject.GetComponent<IPoolable>().OnPool();
        }
        return returnObject;
    }
    /*
    *  DePoolObject
    *  desc: depools [item] of type [id]
    *  paramters: item (the gameobject you wish to depool), id (the type of object it is)
    */
    public void DePoolObject (GameObject item, string id)
    {
        //checks if the object id exists
        if (!pools.ContainsKey(id))
        {
            Errors.LogError("PoolNullDefinition", id);
            return;
        }
        //depools an object of id [id]
        Pool pool = (Pool)pools[id];
        pool.DePoolObject(item);
    }
    /*
    *  DePoolAll
    *  desc: depools all items of type in this pool manager
    */
    public void DePoolAll ()
    {
        //depools all the objects
        foreach (string key in pools.Keys)
        {
            Pool pool = (Pool) pools[key];
            pool.DePoolAll();
        }
    }
}
