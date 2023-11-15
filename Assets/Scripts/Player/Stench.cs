using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stench : MonoBehaviour
{
    public PoolType smellType;

    public float timeToLive = 2f; // Set the time to live in the Inspector    

    private float elapsedTime = 0.0f;
    private ObjectPooler objectPooler;

    public void Start()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    public void Update()
    {
        

        
        if (elapsedTime >= 10.0f)
        {
            // Destroy(gameObject); // Destroy the GameObject when timeToLive is exceeded
            
            objectPooler.AddToPool(smellType, gameObject);
            elapsedTime = 0.0f;
        }

        elapsedTime += Time.deltaTime; // Increment the elapsed time

    }
}
