using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using STMGR;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public float loopTime = 0.0f;
    public NavMeshAgent agent;
    Collider[] playerCollider = new Collider[1];
    public Transform goal;
    public bool seeingPlayer = false;
    public struct PlayerSeen
    {
        public float lastSeenHeard;
        public Vector3 lastPosition;
    };
    public PlayerSeen playerSeen;
    public WaitForSeconds wait = new WaitForSeconds(10.0f);
    public int counter = 0;
    public ENEMY_STATES currentState;

    private float bored = 1.0f;
    // Start is called before the first frame update

    

    private static readonly State[] allStates = new State[5]
    {
      new IdleState(),
      new PatrolState(),
      new ThinkingState(),
      new ChaseState(),
      new AttackState()
    };
    const int PLAYER_MASK = 3;
    const int OBSTRUCTION_MASK = 6;
    const float FOV_TIMEOUT = 0.2f;
    const float THINKING_TIME = 1.0f;
    float fovRadius = 10.0f;
    float fovAngle = 45.0f;
    bool enemyActive = true;
    bool canSeePlayer = false;
    // PatrolController patrolController;
    WaitForSeconds fovWait = new WaitForSeconds(FOV_TIMEOUT);
    WaitForSeconds thinkingWait = new WaitForSeconds(THINKING_TIME);

    // Collider[] fovColliders = new Collider[20];

    void Start()
    {
      // agent = GetComponent<NavMeshAgent>();
      // goal = GetComponent<Transform>();
      StartCoroutine("FOVTimeout");
    }


		private int DrawRandomState()
		{ // uses roulette wheel method
			Random.InitState(System.DateTime.Now.Millisecond);
			switch (Random.value)
			{
				case < 0.5f:
					return (int) ENEMY_STATES.IDLE;
				default:
          return (int) ENEMY_STATES.PATROL;
			}
		}
    private IEnumerator FOVTimeout()
    {
      while(enemyActive)
      {
        yield return fovWait;
        FOVCheck();
      }
    }
    private void onPlayerSpotted()
    {

    }
    private void FOVCheck()
    {
      canSeePlayer = false;
      if (Physics.OverlapSphereNonAlloc(transform.position, fovRadius, playerCollider, PLAYER_MASK) > 0)
      { // if we found player in sphere
        Vector3 directionToPlayer = (playerCollider[0].transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionToPlayer) < fovAngle / 2 )
        { // if within view angle
          if (!Physics.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, OBSTRUCTION_MASK))
          { // if no walls
            Debug.Log("Player seen");
            canSeePlayer = true;
            // update player seen
            playerSeen.lastSeenHeard = Time.time;
            // playerSeen.lastPosition = player.transform.position;
          }
        }
      }
    }
    
    void Update()
    {
      if (loopTime > 2.0f)
      {
        if (canSeePlayer)
        {
          bored = 1.0f; // somethign can happen
          switch (currentState)
          {
            case ENEMY_STATES.CHASE:
              break;
            case ENEMY_STATES.IDLE:
            case ENEMY_STATES.PATROL:
              currentState = ENEMY_STATES.THINK;
              break;
            case ENEMY_STATES.THINK:
              currentState = ENEMY_STATES.CHASE;
              break;
          }
        }
        else
        { // we cannot see player
          if (bored < 0.2f)
          {
            currentState = (ENEMY_STATES)DrawRandomState();
            bored = 1.0f;
          }
        }
        allStates[(int)currentState].exec();
        Debug.Log("Set current to " + currentState.ToString());
        bored -= 0.0001f;
        loopTime = 0.0f;
      }
      loopTime += Time.deltaTime;

    }
    
    void FixedUpdate()
    {
  
    }
    // if player is within a certain distance, start chasing
    // if player is within a certain distance, start attacking
    // if player is within a certain distance, start patrolling
    
    // if player is within a certain distance, start idling

    // using System;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.AI;

// public class Path : MonoBehaviour
// {
  
//   protected readonly Vector3[] NODES = new Vector3[12];

//   public Path()
//   {
//     // collect all nodes from the graph
//     GameObject[] spheres = GameObject.FindGameObjectsWithTag("Node");
//     for (int i = 0; i < 12; i++)
//     {
//         NODES[i] = spheres[i].transform.position;
//         // new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f))
//     }
//   }

//   public Vector3 GetClosestNode(Vector3 pos)
//   {
//     float closestDist = -0.01f; // so that we dont return current position/node
//     Vector3 closest = NODES[0];
//     foreach (Vector3 node in NODES)
//     {
//       float dist = Vector3.Distance(node, pos);
//       if (dist < closestDist)
//       {
//         closestDist = dist;
//         closest = node;
//       }
//     }
//     return closest;
//     // calculate distance to each node
//     // return the closest one
//   }

//   public Vector3[] GeneratePath(int pathLength)
//   {
//     // randomly select n nodes, return them
//     return NODES;
//   }
//   // Start is called before the first frame update
//   // void Start()
//   // {
//   //   = GameObject.FindGameObjectsWithTag("Node");
//   //   
//   // }

//   // void Update()
//   // {
      
//   // }
// }

}