using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  ObjectPooler
 *  desc:
 *  this class is responsible for the functionality of the classes PoolManager and Pool.
 *  make calls to this class to pool objects in your scene as a replacement for GameObject.Instantiate.
 *  the overhead of creating new object is larger than moving already created objects and this system
 *  is designed to allievate this problem
 *  usage:
 *  to pool an object, simply call: ObjectPooler.PoolObject("objectID");
 *  just like GameObject.Instantiate, the object will be returned.
 **/
public class ObjectPooler : MonoBehaviour
{
    public static Hashtable poolManagers = new Hashtable();

    private static string defaultID = "main";

    /*
     * InitializePool
     * desc: initiliazes a pool from the poolManager, 
     * this function is generally only called by a pool manager to sync with the object pooler
     */
    public static void InitilizePool (PoolManager poolManager, string ID)
    {
        poolManagers.Add(ID, poolManager);
    }
    /*
    * PoolObject
    * time: O(1)
    * desc: pools an object of type [objectID] from the default pool
    * parameters: objectID (the id of the object you wish to pool)
    */
    public static GameObject PoolObject(string objectID)
    {
        return PoolObject(defaultID, objectID);
    }
    /*
     * PoolObject
     * time: O(1)
     * desc: pools an object of type [objectID] from pool [poolID]
     * parameters: poolID (the id of the pool which you wish to get the object from), objectID (the id of the object you wish to pool)
     */
    public static GameObject PoolObject(string poolID, string objectID)
    {
        //checks if the object id exists
        if (!poolManagers.ContainsKey(poolID))
        {
            Errors.LogError("MissingDefinition", poolID);
            return null;
        }
        //pools an object of id [id]
        PoolManager poolManager = (PoolManager)poolManagers[poolID];
        GameObject returnObject = poolManager.PoolObject(objectID);
        return returnObject;
    }
    /*
     * DePoolObject
     * time: O(n)
     * desc: will make sure [item] of type [objectID] from [poolID] is ready to be pooled again
     * parameters: item (the gameobject to depool), poolID (the id of the pool which you wish to get the object from), objectID (the id of the object you wish to pool)
     */
    public static void DePoolObject(GameObject item, string poolID, string objectID)
    {
        if (!poolManagers.ContainsKey(poolID))
        {
            Errors.LogError("MissingDefinition", poolID);
            return;
        }
        PoolManager poolManager = (PoolManager)poolManagers[poolID];
        poolManager.DePoolObject(item, objectID);
    }
    /*
     * DePoolAllPools()
     * time: O(n^2)
     * desc: depools all the objects in all the pools
     */
    public static void DePoolAllPools()
    {
        //depools all the poolManagers
        foreach (string key in poolManagers.Keys)
        {
            PoolManager poolManager = (PoolManager)poolManagers[key];
            poolManager.DePoolAll();
        }
    }
    /*
     * DePoolAll()
     * time: O(n)
     * desc: depools all the objects in this pool
     * parameters: poolID (the pool to depool)
     */
    public static void DePoolAll(string poolID)
    {
        //depools the poolManager poolID
        if (!poolManagers.ContainsKey(poolID))
        {
            Errors.LogError("MissingDefinition", poolID);
            return;
        }
        PoolManager poolManager = (PoolManager)poolManagers[poolID];
        poolManager.DePoolAll();
    }
}
