using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using Unity.VisualScripting;
using UnityEditor.VersionControl;

public class CheckPlayerInFieldOfView : Node
{
    // Start is called before the first frame update
    private Transform _transform;
    public FieldOfView fov;
    private static int _playerLayerMask = 1 << 3;
    public bool playerInTrigger = false;
    public float timeToDisengage = 3f;

    public CheckPlayerInFieldOfView(Transform transform, FieldOfView fov)
    {
        _transform = transform;
        this.fov = fov;
    }

    public override NodeState Evaluate()
    {
        object t = getData("target");

        if (fov.canSeePlayer == false && fov.canSmellPlayer == false)
        {
            parent.parent.setData("target", null);
            return NodeState.FAILURE;
        }
        else if (t == null)
        {
            
            if (fov.canSeePlayer == true || fov.canSmellPlayer == true)
            {
                Debug.Log("seeingTarget");
                if (fov.canSeePlayer == true)
                    parent.parent.setData("target", fov.playerLastSeenPostion);
                else if (fov.canSmellPlayer == true) {
                    parent.parent.setData("target", fov.lastSmelledPosition);
                }                

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

