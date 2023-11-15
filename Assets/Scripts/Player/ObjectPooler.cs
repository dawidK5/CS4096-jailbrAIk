using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // so we can use in editor
public enum PoolType { fart, sweat };

[System.Serializable] // so we can use in editor
public class ObjectPool
{
    public PoolType poolType;  // which type of object stored
    public GameObject prefabObject; // holds prefab we use to initialize the type
    public Queue<GameObject> objectQueue;
}

public class ObjectPooler : MonoBehaviour
{
    public ObjectPool[] objectPools;
   
    // Start is called before the first frame update
    void Start()
    {
        foreach(ObjectPool objectPool in objectPools)
        {
            objectPool.objectQueue = new Queue<GameObject>();
        }
    }

    public void AddToPool(PoolType poolType, GameObject item)
    {
        item.SetActive(false); //deactivate object
        FindByPoolType(poolType).objectQueue.Enqueue(item); // pick the pool type and add item to the end
    }

    public GameObject GetFromPool(PoolType poolType)
    {
        ObjectPool objectPool = FindByPoolType(poolType);
        if (objectPool.objectQueue.Count > 0)
        {
            return objectPool.objectQueue.Dequeue(); 
        } else
        {
            return Instantiate(objectPool.prefabObject);
        }
    }

    private ObjectPool FindByPoolType(PoolType poolType)
    {
        ObjectPool result = null;
        foreach (ObjectPool objectPool in objectPools)
        {
            if (objectPool.poolType == poolType)
            {
                result = objectPool;
                
            }
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
