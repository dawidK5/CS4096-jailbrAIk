using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class TaskGoToTargetLook : Node
{
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    

    public TaskGoToTargetLook(Transform transform, NavMeshAgent agent) {
        _navMeshAgent = agent;
        _transform = transform;
    }
    public override NodeState Evaluate()
    {
        Transform target = (Transform)getData("target");

        //_transform.position = _a.MoveTowards(
        //    _transform.position, target.position, SpotLightBT.speed * 2 * Time.deltaTime);
        
        _navMeshAgent.destination = target.position;

            Vector3 vector3 = new Vector3(target.position.x, _transform.position.y, target.position.z);
            _transform.LookAt(vector3);
        
        state =NodeState.RUNNING;
        return state;
     }
}
