using System;
using System.Collections;
using STMGR;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
  [SerializeField]
  public int currentNode = 0;
  public int currentNeighbor = 0;
  const float waitTimeAtNode = 3.0f;
  // bool isPatrolWaiting = false;
  // bool isWaiting = false;
  // bool isMoving = false;

  [SerializeReference]
  public PatrolNode[] P_NODES;
  public EnemyController enemy;
  public NavMeshAgent agent;
  public FSMStatus fsmStatus;
  private float[] waitTimes = new float[1]
  {
    3.0f
  };

  public void AssignNearestNodes()
  {
    for (int i = 0; i < P_NODES.Length; i++)
    {
      for (int j = 0; j < P_NODES.Length; j++)
      {
        if (i != j)
        {
          float dist = (P_NODES[i].location - P_NODES[j].location).sqrMagnitude;
          Debug.Log("Dist " + i + " to " + j + " " + dist);
          if (dist < P_NODES[i].nearNodesRadiusSq)
          {
            Debug.Log("Attempt: add " + j + " to " + i);
            P_NODES[i].AddNear(P_NODES[j]);
          }
        }
      }
    }
  }
  
  void Start()
  {
    // P_NODES = new PatrolNode[9];
    AssignNearestNodes();
    Debug.Log($"--- {(-1)%10}");
  }

  public void Setup(FSMStatus fsmStatus)
  {
    Debug.Log("Agent began patrolling");
    this.fsmStatus = fsmStatus;
    enemy.agent.speed = enemy.DEFAULT_SPEED + 1.0f;
    currentNode = NearestNode();
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
      if (dist <= nearestDistance)
      {
        nearestDistance = dist;
        closestNode = i;
      }
    }
    return closestNode;
  }

  private void NextPatrolPoint()
  { // update target node foward or backward depending on random choice
    switch (UnityEngine.Random.Range(0.0f, 1.0f))
    {
      case < 0.1f:
        PatrolNode p = P_NODES[currentNode];
        currentNeighbor = UnityEngine.Random.Range(0, p.nearNodesIndex);
        break;
      case < 0.9f:
        currentNode = Math.Min(currentNode + 1, P_NODES.Length - 1);
        break;
      default:
        currentNode = Math.Max(currentNode - 1, 0);
        break;
    }
    
  }

  public void Reset()
  {
    currentNode = 0;
  }

  public void RunUpdate(float deltaTime)
  {
    if (enemy.canSeePlayer)
    {
      fsmStatus.nextState = ENEMY_STATES.CHASE;
      fsmStatus.transitionDue = true;
      return;
    }
    if (agent.remainingDistance == 0.0f)
    { // if we are at a node, wait 3s, goto next
      if (waitTimes[0] > 0.0f)
      {
        Debug.Log($"Waiting at patrol node {currentNode}: {waitTimes[0]}s left");
        waitTimes[0] -= deltaTime;
        // Debug.Log("Remaining dist to node: " + agent.remainingDistance);
        return;
      }
      NextPatrolPoint();
      try
      {
        agent.SetDestination(P_NODES[currentNode].location);
      }
      catch
      {
        Debug.Log($"Enemy {this} going to {currentNeighbor}th neighb of {currentNode}");
        agent.SetDestination(P_NODES[currentNode].nearNodes[currentNeighbor].location);
        // Debug.Log($"Enemy {this.name} cannot got to its node {currentNode}");
        // throw new IndexOutOfRangeException();
      }
      waitTimes[0] = waitTimeAtNode;

    }
  }
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

