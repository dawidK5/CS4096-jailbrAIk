using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFart : MonoBehaviour
{
    [SerializeField]
    float fartTime = 0.0f;


    private ObjectPooler objectPooler;

    public PoolType poolType;
    private float createdTime;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        if (fartTime > 5.0f)
        {
 
            //Instantiate(spawnerPrefab, transform.position, Quaternion.identity);
            GameObject newSpawnedObject = objectPooler.GetFromPool(poolType);
            newSpawnedObject.transform.position = transform.position;
            newSpawnedObject.transform.rotation = Quaternion.identity;
            newSpawnedObject.SetActive(true);
            fartTime = 0.0f;

        }
        fartTime += Time.deltaTime;
    }
}
