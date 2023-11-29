using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Transform transformRef;
    public Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        transformRef = GetComponentInParent<Transform>();
        spawnPosition = transformRef.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
