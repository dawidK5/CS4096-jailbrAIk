using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alertable : MonoBehaviour
{
    public Vector3 alertLastPlayerPosition;
    public bool isAlerted = false;
    public bool reachedTarget = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Alert(Vector3 playerPos)
    {
        alertLastPlayerPosition = playerPos;
        isAlerted = true;
    }


}
