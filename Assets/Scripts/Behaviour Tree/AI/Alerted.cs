using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Alerted : Node
{
    private Transform _transform;
    private Alertable _alertScript;
    public bool _reachedTarget = false;


    public Alerted(Transform transform, Alertable alertScript)
    {
        _alertScript = alertScript;
        _transform = transform;
    }
    public override NodeState Evaluate()
    {
        if (_alertScript.isAlerted)
        {
            if(_reachedTarget == false)
            {
                parent.parent.setData("targetPos", _alertScript.alertLastPlayerPosition);
                state = NodeState.SUCCESS;
                return state;
            }
            else
            {
                
                return state;
            }
            
        }
        else
        {
            parent.parent.clearData("targetPos");
            state = NodeState.FAILURE;
            return state;
        }

    }
}
