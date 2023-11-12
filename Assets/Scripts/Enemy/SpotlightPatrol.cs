using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightPatrol : MonoBehaviour
{

    [SerializeReference]
    public Transform[] pNodes;
    private int currentWaypointIndex;
    private float speed = 2f;
    private float waitTime = 1f;
    private float waitCounter = 0f;
    private bool waiting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
        if (waiting)
        {
            waitCounter += Time.deltaTime;
            if(waitCounter < waitTime) {
                return;
            }
            waiting = false;
        }

        Transform wp = pNodes[currentWaypointIndex];
        if (Vector3.Distance(transform.position,wp.position) < 0.1f)
        {
            transform.position = wp.position; ;
            waitCounter = 0f;
            waiting = true;

            currentWaypointIndex = (currentWaypointIndex + 1) % pNodes.Length;
        }
        else
        {
            Debug.Log(Vector3.Distance(transform.position, wp.position) );
            transform.position = Vector3.MoveTowards(transform.position,wp.position,speed * Time.deltaTime);   
        }
    }
}
