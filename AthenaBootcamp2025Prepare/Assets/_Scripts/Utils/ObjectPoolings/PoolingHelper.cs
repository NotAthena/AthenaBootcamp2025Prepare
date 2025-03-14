using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolingHelper : MonoBehaviour
{
    public static List<PoolObjectInfor> ObjectPools = new();

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parent, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (objectToSpawn == null) {
            Debug.Log("Object To Spawn is null!");
            return null;
        }

        PoolObjectInfor pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        //If the pool isn't exist, create it
        if (pool == null)
        {
            pool = new PoolObjectInfor() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        //Check if there are any inactive object in the pool
        GameObject spawnableObject = null;
        if (pool.InactiveObjects.Count > 0)
        {
            spawnableObject = pool.InactiveObjects.FirstOrDefault();
        }

        if (spawnableObject == null)
        {
            spawnableObject = Instantiate(objectToSpawn, parent);
            spawnableObject.transform.SetLocalPositionAndRotation(spawnPosition, spawnRotation);
        }
        else
        {
            spawnableObject.transform.SetParent(parent);
            spawnableObject.transform.SetLocalPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);

        }

        return spawnableObject;
    }

    public static void ReturnObjectToPool(GameObject objectToPool)
    {
        if (objectToPool == null)
        {
            Debug.Log("Object To Return is null!");
            return;
        }
        //Debug.Log("ReturnObjectToPool " + objectToPool.name);
        string objectToPoolName = objectToPool.name.Replace("(Clone)", "").Trim();
        PoolObjectInfor pool = ObjectPools.Find(p => p.LookupString.Equals(objectToPoolName));

        //If the pool isn't exist, create it
        if (pool == null)
        {
            Debug.LogWarning("Release an object that is not pooled!");
            pool = new PoolObjectInfor() { LookupString = objectToPool.name };
            ObjectPools.Add(pool);
        }
        else
        {
            objectToPool.SetActive(false);
            pool.InactiveObjects.Add(objectToPool);
        }

    }
}
[Serializable]
public class PoolObjectInfor
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new();

}