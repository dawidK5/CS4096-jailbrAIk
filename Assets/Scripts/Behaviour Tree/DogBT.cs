using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviourTree;
using Tree = BehaviourTree.Tree;

public class DogBT : Tree
{
    public Transform[] waypoints;
    public NavMeshAgent navMeshAgent;
    public Node root;
    protected override Node SetupTree()
    {
        root = new DogPatrol(transform, waypoints, navMeshAgent);
        return root;
    }
}
