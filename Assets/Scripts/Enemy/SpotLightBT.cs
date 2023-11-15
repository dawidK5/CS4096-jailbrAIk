using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class SpotLightBT : BehaviourTree.Tree 
{
    public UnityEngine.Transform[] waypoints;
    public BTNodeTrigger trigger;
    public static float fovRange = 6f;
    public static float speed = 2f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node> {

            new Sequence(new List<Node>
            {
                new CheckPlayerInTrigger(transform,trigger),
                new TaskGoToTarget(transform),
            }),
            new TaskPatrol(transform, waypoints),
        });
        Debug.Log("Never");
        //Node root = new TaskPatrol(transform, waypoints);
        
        return root;
    }
}
