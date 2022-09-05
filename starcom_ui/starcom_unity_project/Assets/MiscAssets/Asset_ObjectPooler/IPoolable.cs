using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/**
 *  IPoolable
 *  desc:
 *  allows the object pooler to give classes that use this interface additional abilites 
 **/
public interface IPoolable {

    /*
     * OnPool
     * desc: when this interface gets pooled (ie. ObjectPooler.PoolObject() is called), this function is called 
     */
    void OnPool();
    /*
     * OnPool
     * desc: when this interface gets depooled, this function is called 
     */
    void OnDePool();
}
