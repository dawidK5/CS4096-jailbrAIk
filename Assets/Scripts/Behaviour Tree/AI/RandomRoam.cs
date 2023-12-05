using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class RandomRoam : Node
{
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private bool reachedTarget = false;
    private bool inRoaming = false;
    private int roamCountLimit = 3;
    public int roamCount = 0;
    private Vector3 currentSetDestination;
    private Alertable _alertable;
    private float timeLastRoamReached;

    public RandomRoam(Transform transform, NavMeshAgent agent, Alertable alertable)
    {
        _navMeshAgent = agent;
        _transform = transform;
        _alertable = alertable;
    }
    public override NodeState Evaluate()
    {   if (roamCount < roamCountLimit)
        {
            // Initial delay on roam
            if (timeLastRoamReached == 0)
            {
                timeLastRoamReached = Time.fixedTime;
            }
            if (inRoaming == false)
            {
                // Waiting for 3f after each roam
                if (timeLastRoamReached < Time.fixedTime - 2f)
                {
                    Vector3 w = _transform.position;
                    w = Random.insideUnitCircle * 10;
                    w.y = 0;
                    Vector3 position = _transform.position + w;
                    if (NavMesh.SamplePosition(position, out var hit, 10, NavMesh.AllAreas))
                    {

                        _navMeshAgent.SetDestination(hit.position);
                        currentSetDestination = hit.position;
                        inRoaming = true;
                    }
                }
            }
            else if (inRoaming == true)
            {
                Debug.Log(Vector3.Distance(_transform.position, currentSetDestination));
                // Check if reached destination
                if (Vector3.Distance(_transform.position, currentSetDestination) < 1.2f)
                {

                    roamCount += 1;
                    timeLastRoamReached = Time.fixedTime;
                    inRoaming = false;
                }

            }

            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            if (timeLastRoamReached < Time.fixedTime - 2f)
            {
                // Reset alert variables
                this._alertable.isAlerted = false;
                timeLastRoamReached = 0;
                roamCount = 0;
                _alertable.reachedTarget = false;

                state = NodeState.FAILURE;
                return state;
            }
            else
            {
                state = NodeState.SUCCESS;
                return state;
            }
        }
    }
}
