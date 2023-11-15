using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeTrigger : MonoBehaviour
{
    private static int _playerLayerMask = 3;
    public bool playerInTrigger = false;
    public float lastExit = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == _playerLayerMask)
        {
            
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerLayerMask)
        {
            lastExit = Time.fixedTime;
            playerInTrigger = false;
        }
    }
}
