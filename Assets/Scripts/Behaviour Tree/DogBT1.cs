using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviourTree;
using Tree = BehaviourTree.Tree;

public class DogBT1 : Tree
{
    public Transform[] waypoints;
    public NavMeshAgent navMeshAgent;
    public Node root;
    public FieldOfView fov;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node> {
            
            new Sequence(new List<Node>
            {
                new CheckPlayerInFieldOfView(transform,fov),
            }),
            new DogPatrol(transform, waypoints, navMeshAgent),
            
        });

        return root;
    }
}
