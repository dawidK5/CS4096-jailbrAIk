using System;
using System.Collections;
using STMGR;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
  [SerializeField]
  public int currentNode = 0;
  public int currentNeighbor = -1;
  const float waitTimeAtNode = 3.0f;
  // bool isPatrolWaiting = false;
  // bool isWaiting = false;
  // bool isMoving = false;

  [SerializeReference]
  public PatrolNode[] P_NODES;
  public EnemyController enemy;
  public NavMeshAgent agent;
  public FSMStatus fsmStatus;
  private int enemyId = 0;
  private float[] waitTimes = new float[1]
  {
    3.0f
  };
  const float NODE_DIST = 1.5f;

  // public void AssignNearestNodes()
  // {
  //   for (int i = 0; i < P_NODES.Length; i++)
  //   {
  //     for (int j = 0; j < P_NODES.Length; j++)
  //     {
  //       if (P_NODES[i].nodeId != P_NODES[j].nodeId)
  //       {
  //         float dist = (P_NODES[i].location - P_NODES[j].location).sqrMagnitude;
  //         Debug.Log("Dist " + i + " to " + j + " " + dist);
  //         if (dist < P_NODES[i].nearNodesRadiusSq)
  //         {
  //           Debug.Log("Attempt: add " + j + " to " + i);
  //           P_NODES[i].AddNear(P_NODES[j]);
  //         }
  //       }
  //     }
  //   }
  // }
  
  void Start()
  {
    // P_NODES = new PatrolNode[9];
    // AssignNearestNodes();
  }

  public void Setup(FSMStatus fsmStatus)
  {
    enemyId = enemy.enemyId;
    Debug.Log($"E_{enemyId}@S: began patrolling");
    this.fsmStatus = fsmStatus;
    enemy.agent.speed = enemy.DEFAULT_SPEED + 1.0f;
    currentNode = NearestNode();
    currentNeighbor = -1;
    agent.SetDestination(P_NODES[currentNode].location);
  }

  private int NearestNode()
  {
    // Return the node nearest to the enemy
    float nearestDistance = 99999.0f;
    int closestNode = 0;
    for (int i = 0; i < P_NODES.Length; i++)
    {
      float dist = Vector3.Distance(P_NODES[i].location, enemy.transform.position);
      if (dist <= nearestDistance && dist > NODE_DIST)
      {
        nearestDistance = dist;
        closestNode = i;
      }
    }
    return closestNode;
  }

  private void NextPatrolPoint()
  { // update target node foward or backward depending on random choice
    bool nodeBusy = true;
    while(nodeBusy)
    { // while enemy at the node
      switch (UnityEngine.Random.Range(0.0f, 1.0f))
      {
        case < 0.2f:
          // go to neighbour of current node
          PatrolNode p = P_NODES[currentNode];
          currentNeighbor = UnityEngine.Random.Range(0, p.nearNodesIndex);
          // Debug.Log($"E_{enemyId}@NPP: try neighbour {currentNeighbor} of PN_{p.nodeId}");
          nodeBusy = P_NODES[currentNode].nearNodes[currentNeighbor].occupied;
          // Debug.Log($"E_{enemyId}@NPP: try neighbour | success:{!nodeBusy}");
          break;
        case < 0.8f:
          currentNeighbor = -1;
          if (currentNode+1 < P_NODES.Length)
          {
            currentNode += 1;
          }
          else
          {
            currentNode = 0;
          }
          nodeBusy = P_NODES[currentNode].occupied;
          // Debug.Log($"E_{enemyId}@NPP: try next node | success:{!nodeBusy}");
          break;
        default:
          currentNeighbor = -1;
          currentNode = Math.Max(currentNode - 1, 0);
          nodeBusy = P_NODES[currentNode].occupied;
          Debug.Log($"E_{enemyId}@NPP: try previous node | success:{!nodeBusy}");
          break;
      }  
    }
    
  }

  public void Reset()
  {
    if (currentNeighbor == -1)
    {
      P_NODES[currentNode].occupied = false;
    }
    else
    {
      P_NODES[currentNode].nearNodes[currentNeighbor].occupied = false;
    }
    currentNode = 0;
    currentNeighbor = -1;
  }

  public void RunUpdate(float deltaTime)
  {
    if (enemy.canSeePlayer)
    {
      fsmStatus.nextState = ENEMY_STATES.CHASE;
      fsmStatus.transitionDue = true;
      return;
    }
    if (agent.remainingDistance < NODE_DIST)
    { // if we are at a node, wait 3s, goto next
      if (waitTimes[0] > 0.0f)
      {
        // Debug.Log($"E_{enemyId}@RU: Waiting at patrol node {currentNode}: {waitTimes[0]}s left");
        waitTimes[0] -= deltaTime;
        // Debug.Log("Remaining dist to node: " + agent.remainingDistance);
        return;
      }
      // after waiting, free the node
      if (currentNeighbor == -1)
      {
        P_NODES[currentNode].occupied = false;
      }
      else
      {
        P_NODES[currentNode].nearNodes[currentNeighbor].occupied = false;
      }
      NextPatrolPoint();
      if (currentNeighbor == -1)
      {
        // go to current, assume did not get occupied in mean time
        agent.SetDestination(P_NODES[currentNode].location);
        P_NODES[currentNode].occupied = true;
      }
      else
      {
        // go to neighbour
        agent.SetDestination(P_NODES[currentNode].nearNodes[currentNeighbor].location);
        P_NODES[currentNode].nearNodes[currentNeighbor].occupied = true;
      }
      
        // Debug.Log($"Enemy {this.name} cannot got to its node {currentNode}");
        // throw new IndexOutOfRangeException();
      }
      waitTimes[0] = waitTimeAtNode;
    }
    // if (agent.remainingDistance < 4.0f)
    // { // if very close to the node, take it over
    //   if(currentNeighbor == -1)
    //   {
    //     P_NODES[currentNode].occupied = false;
    //   }
    //   else
    //   {
    //     P_NODES[currentNode].nearNodes[currentNeighbor].occupied = false;
    //   }
    // }
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

