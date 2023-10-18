using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
  // [SerializeField]
  public int currentNode = 0;
  float waitTimeAtNode = 3.0f;
  bool isPatrolWaiting = false;
  bool isWaiting = false;
  bool isMoving = false;

  // [SerializeReference]
  public PatrolNode[] P_NODES = new PatrolNode[9];
  GameObject[] NODES = new GameObject[9];
  public NavMeshAgent agent;

  public void AssignNearestNodes()
  {
    for (int i = 0; i < P_NODES.Length; i++)
    {      
      for (int j = 0; j < P_NODES.Length; j++)
      {
        if (i != j)
        {
          // float dist = 
          Debug.Log($"Dist {i} to {j}: {Vector3.Distance(P_NODES[i].location, P_NODES[j].location)}");
          if (Vector3.Distance(P_NODES[i].location, P_NODES[j].location) < P_NODES[i].nearNodesRadius)
          {
            P_NODES[i].nearNodes.Add(P_NODES[j]);
          }
        }
      }
    }
  }

  public void Start()
  {
    NODES = GameObject.FindGameObjectsWithTag("Node");
    for (int i = 0; i < NODES.Length; i++)
    {
      Debug.Log($"{i}th sphere found");
      P_NODES[i] = NODES[i].GetComponent<PatrolNode>();
    }
    agent = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NavMeshAgent>();
    Debug.Log("Agent is " + agent.ToString());
    AssignNearestNodes();
    // UpdateDestination();
  }


  // public void Update()
  // {
  //   if (isMoving && agent.remainingDistance < 1.0f)
  //   { // moving close to the patrol point
  //     isMoving = false;
  //     if (isPatrolWaiting)
  //     {
  //       isWaiting = true;
  //     }
  //     else
  //     {
  //       ChangePatrolPoint();
  //       UpdateDestination();
  //     }
      
  //   }
  // }

  private void ChangePatrolPoint()
  { // go foward or backward depending on random choice
    currentNode = (currentNode + (UnityEngine.Random.Range(0.0f, 1.0f) < 0.1f ? -1 : 1)) % NODES.Length;
  }

  private void UpdateDestination()
  {
    agent.SetDestination(P_NODES[currentNode].location);
  }

  // public Vector3 GetClosestNode(Vector3 pos)
  // {
  //   float closestDist = -0.01f; // so that we dont return current position/node
  //   Vector3 closest = NODES[0];
  //   foreach (Vector3 node in NODES)
  //   {
  //     float dist = Vector3.Distance(node, pos);
  //     if (dist < closestDist)
  //     {
  //       closestDist = dist;
  //       closest = node;
  //     }
  //   }
  //   return closest;
  //   // calculate distance to each node
  //   // return the closest one
  // }

  // public Vector3[] GeneratePath(int pathLength)
  // {
  //   // randomly select n nodes, return them
  //   return NODES;
  // }
  // Start is called before the first frame update
  // void Start()
  // {
  //   = GameObject.FindGameObjectsWithTag("Node");
  //   
  // }

  // void Update()
  // {

  // }
}
