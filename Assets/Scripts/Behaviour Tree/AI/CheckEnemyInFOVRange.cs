using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class CheckEnemyInFOVRange : Node
{
    // Start is called before the first frame update
    private Transform _transform;

    private static int _playerLayerMask = 1 << 3;

    public CheckEnemyInFOVRange(Transform transform)
    {
        _transform = transform; 
    }

    public override NodeState Evaluate()
    {
        object t = getData("target");
        if (t == null) {
            
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, SpotLightBT.fovRange, _playerLayerMask);
            
            if(colliders.Length > 0) 
            {
                parent.parent.setData("target", colliders[0].transform);

                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
