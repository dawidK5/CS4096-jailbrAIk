using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using UnityEngine.AI;

public class IdleEnemyBT : BehaviourTree.Tree
{
    public UnityEngine.Transform[] waypoints;
    public SpawnPoint spawnpoint; 
    public FieldOfView fov;
    public NavMeshAgent navMeshAgent;
    public Alertable alertScript;

    public static float fovRange = 6f;
    public static float speed = 2f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node> {

            new Sequence(new List<Node>
            {
                new CheckPlayerInFOV(transform,fov),
                new TaskGoToTargetLook(transform,navMeshAgent,alertScript),
            }),
            new Sequence(new List<Node>
            {
                new Alerted(transform,alertScript),
                new Selector(new List<Node>
                {
                      new GoToLastTargetPosition(transform,navMeshAgent, alertScript),
                      new RandomRoam(transform,navMeshAgent, alertScript),
                }),
                
            }),
            new GoBackToSpawnPoint(transform, spawnpoint.spawnPosition,navMeshAgent),
        });

        return root;
    }
}
