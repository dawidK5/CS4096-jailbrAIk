using System;
using System.Collections.Generic;
using STMGR;
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
  private bool[] rotationsDone = new bool[3] {false,false,false};
  // private bool  isRotationRunning
  private static readonly float[] WAIT_TIMES = new float[3]
  {
    2.0f, 2.0f, 2.0f,
  };
  private float[] waitTimes = new float[3]
  {
    2.0f, 2.0f, 2.0f,
  };
  private static readonly float[] ROTATIONS = new float[3] {-50.0f, 100.0f, -50.0f}; // offset from forward
  private static readonly float[] ROTATION_TIMES = new float[3] {1.5f, 3.0f, 1.5f };
  const float NODE_DIST = 1.5f;

  public void Setup(FSMStatus fsmStatus, EnemyController myEnemy)
  {
    this.enemy = myEnemy;
    // Debug.Log($"Setting up enemy, current state: {this.enemy.currentState}, can see: {this.enemy.canSeePlayer}");
    enemyId = enemy.enemyId;
    // Debug.Log($"E_{enemyId}@S: began patrolling");
    this.fsmStatus = fsmStatus;
    ResetRotations();
    // enemy.agent.speed = enemy.DEFAULT_SPEED + 1.0f;
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
          try
          {
            nodeBusy = P_NODES[currentNode].nearNodes[currentNeighbor].occupied;
            // Debug.Log($"E_{enemyId}@NPP: try neighbour | success:{!nodeBusy}");
          }
          catch
          {
            List<string> allNeighbours = new List<string>(); 
            foreach (PatrolNode pn in P_NODES[currentNode].nearNodes)
            {
              allNeighbours.Add(pn.nodeId.ToString());
            }
            // Debug.Log($"E_{enemyId}@NPP N_{P_NODES[currentNode].nodeId} has no neighbours, will draw again");
          }
          break;
        case < 0.8f:
          // go to next patrol node
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
          // if bored go to previous node
          if (enemy.bored < 0.5f)
          {
            currentNeighbor = -1;
            currentNode = Math.Max(currentNode - 1, 0);
            nodeBusy = P_NODES[currentNode].occupied;
          }
          else
          {
            nodeBusy = true; // draw again
          }
          // Debug.Log($"E_{enemyId}@NPP: try previous node | success:{!nodeBusy}");
          break;
      }  
    }
    
  }

  public void Reset()
  {
    ResetRotations();
    for (int i = 0; i < 3; i++)
    {
      waitTimes[i] = WAIT_TIMES[i];
    }
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

  private void ResetRotations()
  {
    for (int i = 0; i < 3; i++)
    {
      waitTimes[i] = WAIT_TIMES[i];
      rotationsDone[i] = false;
    }
  }
  public void RunUpdate(float deltaTime)
  {
//    Debug.Log(this.enemy.currentState);
    if (this.enemy.canSeePlayer)
    {
      fsmStatus.nextState = ENEMY_STATES.CHASE;
      fsmStatus.transitionDue = true;
      return;
    }
    // wait for enemy to finish slerp
    if (enemy.isTurning || enemy.isRotating)
    {
      return;
    }
    if (agent.remainingDistance < NODE_DIST)
    { // if we are at a node
      // stop fully
      if (!agent.isStopped)
      {
        agent.ResetPath();
        agent.isStopped = true;
        ResetRotations();
        // Debug.Log("Start SLERP");
        enemy.RotateToInterestingSpace();
        return;
      }
      // Debug.Log($"enemy turning: {enemy.isTurning}");
      // then complete rotations at node
      for (int i = 0; i < 3; i++)
      {
        if (!rotationsDone[i])
        {
          // Debug.Log($"Start Rotation {i}");

          enemy.RotateTo(ROTATIONS[i], ROTATION_TIMES[i]);
          // Debug.Log($"Rotation {i} done");
          rotationsDone[i] = true;
          return;
        }
        if (waitTimes[i] > 0.0f)
        {
          waitTimes[i] -= deltaTime;
          return;
        }
      }
      // After lookinng, get next node
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
        agent.isStopped = false;
      }
      else
      {
        // go to neighbour
        agent.SetDestination(P_NODES[currentNode].nearNodes[currentNeighbor].location);
        P_NODES[currentNode].nearNodes[currentNeighbor].occupied = true;
        agent.isStopped = false;
      }
    }
  }
}
    
      // if (waitTimes[0] > 0.0f)
      // {
      //   if (!enemy.isRotating)
      //   {
      //     enemy.rotateBy = -170.0f;
      //     enemy.isRotating = true;
      //     return;
      //   }
        //        Debug.Log($"E_{enemyId}@RU: Waiting at patrol node {currentNode}: {waitTimes[0]}s left");
        // agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.Euler(0, -170.0f, 0), 2.0f-waitTimes[0]);
        // waitTimes[0] -= deltaTime;
        // Debug.Log("Remaining dist to node: " + agent.remainingDistance);
        // return;
      // }
      // if (waitTimes[1] > 0.0f)
      // {
      //   if (!enemy.isRotating)
      //   {
      //     enemy.rotateBy = 340.0f;
      //     enemy.isRotating = true;
      //     return;
      //   }
      //   //        Debug.Log($"E_{enemyId}@RU: Waiting at patrol node {currentNode}: {waitTimes[0]}s left");
      //   // agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.Euler(0, 340.0f, 0), 4.0f-waitTimes[1]);
      //   waitTimes[1] -= deltaTime;
      //   // Debug.Log("Remaining dist to node: " + agent.remainingDistance);
      //   return;
      // }
      // if (waitTimes[2] > 0.0f)
      // {
      //   if (!enemy.isRotating)
      //   {
      //     enemy.rotateBy = -170.0f;
      //     enemy.isRotating = true;
      //     return;
      //   }
      //   //        Debug.Log($"E_{enemyId}@RU: Waiting at patrol node {currentNode}: {waitTimes[0]}s left");
      //   //agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.Euler(0, -170.0f, 0), 2.0f - waitTimes[1]);
      //   waitTimes[2] -= deltaTime;
      //   // Debug.Log("Remaining dist to node: " + agent.remainingDistance);
      //   return;
      // }
      // if (waitTimes[0] > 0.0f)
      // {
      //   //        Debug.Log($"E_{enemyId}@RU: Waiting at patrol node {currentNode}: {waitTimes[0]}s left");
      //   waitTimes[0] -= deltaTime;
      //   // Debug.Log("Remaining dist to node: " + agent.remainingDistance);
      //   return;
      // }
      //Quaternion.RotateTowards(Quaternion.LookRotation(agent.transform.forward), Quaternion.LookRotation(agent.transform.right), 8.0f);
      // after waiting, free the node
      // Free up the node
      
      
        // Debug.Log($"Enemy {this.name} cannot got to its node {currentNode}");
        // throw new IndexOutOfRangeException();
      
      // waitTimes[0] = 2.0f;
      // waitTimes[1] = 4.0f;
      // waitTimes[2] = 2.0f;
    
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

