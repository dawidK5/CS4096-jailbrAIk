using UnityEngine;
using UnityEngine.AI;
using STMGR;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour
{
  [Header("References")]
  public float loopTime = 0.0f;
  public NavMeshAgent agent;
  public Transform player;
  public EnemyPatrol patrol;
  public bool canSeePlayer = false;
  public bool canCapturePlayer = false;
  public bool sensingRunning;

  public struct PlayerSeen
  {
      public float lastSeenHeard;
      public Vector3 lastPosition;
  };
  public PlayerSeen playerSeen;
  public ENEMY_STATES currentState;
  // Motivational buckets
  public float bored = 1.0f;
  public float tired = 1.0f;
  public float senseInterval;
  private static readonly State[] allStates = new State[3];

  // const int PLAYER_MASK = 3;
  const int OBSTRUCTION_MASK = 6;
  float fovRadiusSq = 100.0f;
  float fovHalfAngle = 45.0f;
  public readonly float DEFAULT_SPEED = 3.0f;
  Vector3 directionToPlayer;



  public void FixedUpdate()
  {
    if (senseInterval > 0.25f)
    {
      sensingRunning = true;
      FOVCheck();
      sensingRunning = false;
      senseInterval = 0.0f;
      bored -= 0.03f;
    }
    senseInterval += Time.fixedDeltaTime;
  }

  void FOVCheck()
  {
    canSeePlayer = false;
    directionToPlayer = player.position - transform.position;
    if (directionToPlayer.sqrMagnitude < fovRadiusSq)
    { // if we found player in sphere
      if (Vector3.Angle(transform.forward, directionToPlayer.normalized) < fovHalfAngle)
      { // if within view angle
        if (!Physics.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, OBSTRUCTION_MASK))
        { // if no walls
          Debug.DrawRay(transform.position, directionToPlayer, Color.red);
          Debug.Log("Player seen");
          canSeePlayer = true;
          // update player seen
          playerSeen.lastSeenHeard = Time.time;
          playerSeen.lastPosition = player.position;
          if (directionToPlayer.sqrMagnitude < 2.0f)
          {
            canCapturePlayer = true;
            Debug.Log("Player captured");
          }
        }
        else
        {
          Debug.Log("Player behind a wall");
        }
      }
    }
    // if (!canSeePlayer)
    // {
    //   Debug.Log("Player not seen");
    // }
  }




}