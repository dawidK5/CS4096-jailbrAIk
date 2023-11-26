using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class GoBackToSpawnPoint : Node
{
    private Transform _transform;
    private Vector3 _spawnPosition;
    private NavMeshAgent _navMeshAgent;

    public GoBackToSpawnPoint(Transform transform, Vector3 spawnPosition, NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
        _transform = transform;
        _spawnPosition = spawnPosition;
    }
    public override NodeState Evaluate()
    {
        _navMeshAgent.destination = _spawnPosition;

        
        state = NodeState.RUNNING;
        return state;
    }
}
