using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
  [SerializeField]
  public NavMeshAgent agent;
  public PatrolNode[] NODES = new PatrolNode[12];
  float waitTimeAtNode = 3.0f;
  bool isPatrolWaiting = false;
  bool isWaiting = false;
  int currentNode = 0;
  bool isMoving = false;

  
  private void AssignNearestNodes()
  {
    GameObject[] spheres = GameObject.FindGameObjectsWithTag("Node");
    foreach (GameObject s in spheres)
    {
      Debug.Log(s.ToString());
      NODES[0].location = s.transform.position;
    }
    // for (int i = 0; i < spheres.Length; i++)
    // {
    //   // print the sphere number

    //   Debug.Log(i + "th sphere found", spheres[i]);
    //   NODES[i].location = spheres[i].transform.position;
    // }
    // for (int i = 0; i < NODES.Length; i++)
    // {
    //   for (int j = 0; j < NODES.Length; j++)
    //   {
    //     if (i != j)
    //     {
    //       if (Vector3.Distance(NODES[i].location, NODES[j].location) < NODES[i].nearNodesRadius)
    //       {
    //         NODES[i].nearestNodes.Add(NODES[j]);
    //       }
    //     }
    //   }
    // }
  }
  public void Start()
  {
    // agent = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NavMeshAgent>();
    Debug.Log("Agent is " + agent.ToString());
    AssignNearestNodes();
    UpdateDestination();
  }

  public void Update()
  {
    if (isMoving && agent.remainingDistance < 1.0f)
    { // moving close to the patrol point
      isMoving = false;
      if (isPatrolWaiting)
      {
        isWaiting = true;
      }
      else
      {
        ChangePatrolPoint();
        UpdateDestination();
      }
      
    }
  }

  private void ChangePatrolPoint()
  { // go foward or backward depending on random choice
    currentNode = (currentNode + (UnityEngine.Random.Range(0.0f, 1.0f) < 0.1f ? -1 : 1)) % NODES.Length;
  }

  private void UpdateDestination()
  {
    agent.SetDestination(NODES[currentNode].location);
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
