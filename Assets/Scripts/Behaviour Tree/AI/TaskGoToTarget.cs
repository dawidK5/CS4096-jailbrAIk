using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
public class TaskGoToTarget : Node
{
    private Transform _transform;

    

    public TaskGoToTarget(Transform transform) {

        _transform = transform;
    }
    public override NodeState Evaluate()
    {
        Transform target = (Transform)getData("target");

        if (Vector3.Distance(_transform.position, target.position) > 0.1f)
        {
            
            _transform.position = Vector3.MoveTowards(
                _transform.position, target.position, SpotLightBT.speed * 2 * Time.deltaTime);
            //_transform.LookAt(target.position);
            
        }
        state =NodeState.RUNNING;
        return state;
     }
}
