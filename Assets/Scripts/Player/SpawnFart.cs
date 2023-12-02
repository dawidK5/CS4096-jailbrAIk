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
    public float maxFartometer = 1f;
    public float currentFartometer = 0f;
    public Fartometer fartometer;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();

    }
private float fartInterval = 5.0f; // damage interval in seconds
private float decrementPerSecond; 
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
            currentFartometer = maxFartometer;
            fartometer.setMaxFartometer(maxFartometer);
            fartTime = 0.0f;
            decrementPerSecond = currentFartometer / fartInterval;
        }
        
        fartTime += Time.deltaTime;
    
    float decrementPerFrame = decrementPerSecond * Time.deltaTime;
    takeFartometerDamage(decrementPerFrame);
    }
    void takeFartometerDamage(float damage)
    {
        currentFartometer -= damage;
        fartometer.setFartometer(currentFartometer);
    }
}
