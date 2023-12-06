using UnityEngine;
using UnityEngine.AI;
using STMGR;
using UnityEditor;
using System;

public class EnemyController : MonoBehaviour
{
  [Header("References")]
  public NavMeshAgent agent;
  public Transform player;
  public Rigidbody playerRb;
  public EnemyPatrol patrol;
  public int enemyId = 0;
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
  // const int PLAYER_MASK = 3;
  const int OBSTRUCTION_MASK = 1 << 6;
  const float fovRadiusSq = 205.25f;
  public readonly float fovHalfAngle = 47.5f;
  public readonly float DEFAULT_SPEED = 2.5f;
  private Vector3 directionToPlayer;
  public FieldOfView fov;

  void Awake()
  {
    enemyId = Utils.getId(name);
  }

  public void FixedUpdate()
  {
    if (senseInterval > 0.125f)
    {
      sensingRunning = true;
      FOVCheck();
      sensingRunning = false;
      senseInterval = 0.0f;
      if (bored > 0.0f)
      {
        bored -= 0.03f;
      }
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
          // Steer towards where the player will be

          // directionToPlayer.normalized
          // float relativeHeading = Vector3.Dot(transform.forward, playerRb.velocity.normalized);
          // if (relativeHeading < 0.0f)
          // {
          //   // 
          // }
          // else
          // {

          // }

          // if (directionToPlayer.sqrMagnitude < 2.0f)
          // {
          //   canCapturePlayer = true;
          //   Debug.Log($"EC@FOVC: Player captured by E_{enemyId}");
          // }
          // while(canSeePlayer)
          // {
          //   player.forward
          // }
        }
        else
        {
          Debug.Log($"EC@FOVC: E_{enemyId} player not detected (behind wall)");
        }
      }
    }
    // if (!canSeePlayer)
    // {
    //   Debug.Log("Player not seen");
    // }
  }
}