using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class DogPatrol : Node
{
    private Transform _transform;
    private Transform[] _waypoints;
    private int _currentWaypointIndex = 0;
    private float _waitTime = 20f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;
    private NavMeshAgent _navMeshAgent;
    Vector3 target;
    public DogPatrol(Transform transform, Transform[] waypoints, NavMeshAgent navMeshAgent)
    {
        _transform = transform;
        _waypoints = waypoints;
        _navMeshAgent = navMeshAgent;
        UpdateDestination();
    }
   
    public override NodeState Evaluate()
    {
        if(_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
                _waiting = false;
        }
        else
        {
            
            if (Vector3.Distance(_transform.position, target) < 0.8f)
            {
                _waitCounter = 0f;
                _waiting = true;
                IterateWaypointIndex();
                UpdateDestination();
            }
        }
        state = NodeState.RUNNING;
        return state;
    }

    public void UpdateDestination()
    {
        target = _waypoints[_currentWaypointIndex].position;
        _navMeshAgent.SetDestination(target);
    }

    void IterateWaypointIndex()
    {
        _currentWaypointIndex++;
        if (_currentWaypointIndex == _waypoints.Length)
        {
            _currentWaypointIndex = 0;
        }
    }
}
