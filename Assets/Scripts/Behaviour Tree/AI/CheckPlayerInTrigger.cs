using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using Unity.VisualScripting;
using UnityEditor.VersionControl;

public class CheckPlayerInTrigger : Node
{
    // Start is called before the first frame update
    private Transform _transform;
    public BTNodeTrigger bTNodeTrigger;
    private static int _playerLayerMask = 1 << 3;
    public bool playerInTrigger = false;
    public float timeToDisengage = 3f;

    public CheckPlayerInTrigger(Transform transform, BTNodeTrigger bTNodeTrigger)
    {
        _transform = transform;
        this.bTNodeTrigger = bTNodeTrigger;
    }

    public override NodeState Evaluate()
    {
        object t = getData("target");
 
        // Clearing target and returning fail to stop following player after period of time not in trigger.
        if (bTNodeTrigger.playerInTrigger == false && bTNodeTrigger.lastExit < Time.fixedTime - timeToDisengage)
        {
            parent.parent.setData("target", null);
            return NodeState.FAILURE;
        }
        else if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, SpotLightBT.fovRange, _playerLayerMask);

            if (bTNodeTrigger.playerInTrigger == true)
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
