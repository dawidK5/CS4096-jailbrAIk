using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using Unity.VisualScripting;

public class TaskPatrol : Node
{
    private Transform _transform;


    [SerializeReference]
    public Transform[] pNodes;
    private int currentWaypointIndex = 0;

    private float waitTime = 1f;
    private float waitCounter = 0f;
    private bool waiting = false;

    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        _transform = transform;
        pNodes = waypoints;
    }

    public override NodeState Evaluate()
    {
        if (waiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                waiting = false;
            }
            
        }
        else
        {

            Transform wp = pNodes[currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 0.1f)
            {
                _transform.position = wp.position; ;
                waitCounter = 0f;
                waiting = true;

                currentWaypointIndex = (currentWaypointIndex + 1) % pNodes.Length;
            }
            else
            {
                _transform.position = Vector3.MoveTowards(_transform.position, wp.position, SpotLightBT.speed * Time.deltaTime);
            }
        }

        state = NodeState.RUNNING; 
        return state;
    }
}
