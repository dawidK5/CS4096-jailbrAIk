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
    private Alertable _alertable;

    public TaskGoToTargetLook(Transform transform, NavMeshAgent agent, Alertable alertable) {
        _navMeshAgent = agent;
        _transform = transform;
        _alertable = alertable;
    }
    public override NodeState Evaluate()
    {
        Transform target = (Transform)getData("target");

        //_transform.position = _a.MoveTowards(
        //    _transform.position, target.position, SpotLightBT.speed * 2 * Time.deltaTime);
        
        _navMeshAgent.destination = target.position;
        _alertable.isAlerted = false;
        _alertable.reachedTarget = false;
        Vector3 vector3 = new Vector3(target.position.x, _transform.position.y, target.position.z);
        _transform.LookAt(vector3);
        
        state =NodeState.RUNNING;
        return state;
     }
}
