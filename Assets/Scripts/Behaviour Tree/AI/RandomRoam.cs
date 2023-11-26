using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomRoam : Node
{
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private bool reachedTarget = false;
    private bool roaming;

    public RandomRoam()
    {
        //_navMeshAgent = agent;
        //_transform = transform;
    }
    public override NodeState Evaluate()
    {
        Debug.Log("INROAMING");
        state = NodeState.SUCCESS;
        return state;
    }
}
