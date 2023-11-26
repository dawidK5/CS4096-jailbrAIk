using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class GoToLastTargetPosition : Node
{ 
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private bool roaming;
    private Alertable _alertScript;

    public GoToLastTargetPosition(Transform transform, NavMeshAgent agent, Alertable alertScript)
    {
        _navMeshAgent = agent;
        _transform = transform;
        _alertScript = alertScript;
    }
    public override NodeState Evaluate()
    {
        Vector3 target = (Vector3)getData("targetPos");
        
        if (_alertScript.reachedTarget == false)
        {

            if (Vector3.Distance(_transform.position, target) < 5f)
            {
                _alertScript.reachedTarget = true;
                parent.parent.setData("targetPos", null);
                state = NodeState.FAILURE;
                return state;
            }
            _navMeshAgent.destination = target;
            state = NodeState.SUCCESS;
            return state;
        }
        Debug.Log("roaming Reached :" + _alertScript.reachedTarget);
        state = NodeState.FAILURE;
        return state;



    }
}



   