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
  public bool isRotating = false;

  public struct PlayerSeen
  {
      public float lastSeenHeard;
      public Vector3 lastPosition;
  };
  public PlayerSeen playerSeen;
  public ENEMY_STATES currentState;
  // Motivational buckets
  public float bored = 1.0f;
  public float deltaBored = 0.0f;
  public float tired = 1.0f;
  public float deltaTired = 0.0f;
  
  public float senseInterval;
  const int DOOR_OBSTRUCTION_MASK = (1 << 6) | (1 << 7);
  const float fovRadiusSq = 205.25f;
  
  public readonly float fovHalfAngle = 47.5f;
  public float DEFAULT_SPEED = 5.0f;
  public float DEFAULT_ACCELERATION = 6.0f;
  
  private Vector3 directionToPlayer;
  public Vector3 target;
  public float rotateBy = 0.0f;
  public float rotationProgress;
  
  Quaternion afterRotation;
  Quaternion interestingDirection;
  Quaternion beforeSlerp;
  public bool isTurning = false;
  float rotationStep, rotationStepAbs;

  void Awake()
  {
    enemyId = Utils.getId(name);
  }

  public void RotateTo(float degrees, float rotationTime)
  {
    if (!canSeePlayer && !isTurning && !isRotating)
    {
      agent.ResetPath();
      agent.isStopped = true;
      rotateBy = degrees;
      rotationProgress = Math.Abs(rotateBy);
      rotationStep = (degrees / rotationTime) * Time.fixedDeltaTime;
      rotationStepAbs = Math.Abs(rotationStep);
      isRotating = true;
      // Debug.Log($"Rotation step {rotationStep}");
    }

  }

  public void RotateToInterestingSpace()
  {
    if(!canSeePlayer && !isTurning && !isRotating)
    {
      agent.ResetPath();
      agent.isStopped = true;
      beforeSlerp = agent.transform.rotation;
      interestingDirection = GetInterestingDirection();
      isTurning = (agent.transform.rotation.eulerAngles.y == interestingDirection.eulerAngles.y);
    }
  }

  private Quaternion GetInterestingDirection()
  {
    Debug.DrawRay(agent.transform.position, agent.transform.forward, Color.red);
    if (!Physics.Raycast(agent.transform.position, agent.transform.forward, 2.5f, 1 << 6))
    { // if no wall in front of us
      // Debug.Log("No wall");
      return agent.transform.rotation;
    }
    Debug.DrawRay(agent.transform.position, Vector3.forward, Color.blue);

    if (!Physics.Raycast(agent.transform.position, Vector3.forward, 2.5f, 1 << 6))
    {
      // Debug.Log("North");
      return Quaternion.Euler(Vector3.forward);
    }
    Debug.DrawRay(agent.transform.position, -Vector3.forward, Color.yellow);

    if (!Physics.Raycast(agent.transform.position, -Vector3.forward, 2.5f, 1 << 6))
    {
      // Debug.Log("South");
      return Quaternion.Euler(-Vector3.forward);
    }
    Debug.DrawRay(agent.transform.position, -Vector3.left, Color.white);

    if (!Physics.Raycast(agent.transform.position, Vector3.left, 2.5f, 1 << 6))
    {
      // Debug.Log("West");
      return Quaternion.Euler(Vector3.left);
    }
    Debug.DrawRay(agent.transform.position, -Vector3.left, Color.black);
    // Debug.Log("East");
    return Quaternion.Euler(-Vector3.left);

  }

  private float totalTurned = 0.0f;
  public void FixedUpdate()
  {
    if (senseInterval > 0.125f)
    {
      FOVCheck();
      senseInterval = 0.0f;
      if (bored > 0.0f)
      {
        bored -= deltaBored;
      }
      if (tired > 0.0f)
      {
        tired -= deltaTired;
      }
    }
    senseInterval += Time.fixedDeltaTime;
    if (!canSeePlayer)
    {
      if (isTurning)
      {
        float angle = 50.0f * Time.fixedDeltaTime;
        agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, interestingDirection, angle);
        Debug.Log(angle);
        if (Quaternion.Angle(agent.transform.rotation, interestingDirection) < 1.0f || canSeePlayer)
        {
          isTurning = false;
          agent.ResetPath();
          agent.isStopped = true;
          return;
        }
        return;
      }
      if (isRotating)
      {
        agent.transform.Rotate(Vector3.up * rotationStep);
        rotationProgress -= rotationStepAbs;
        if (rotationProgress <= 0 || canSeePlayer)
        {
          isRotating = false;
          agent.ResetPath();
          agent.isStopped = true;
          // Debug.Log($"Rotation finished at {agent.transform.rotation.eulerAngles.y} agent-degrees");
        }
      }
    }
  }

  void FOVCheck()
  {
    // canSeePlayer = false;
    directionToPlayer = player.position - transform.position;
    if (directionToPlayer.sqrMagnitude < fovRadiusSq)
    { // if we found player in sphere
      if (Vector3.Angle(transform.forward, directionToPlayer.normalized) < fovHalfAngle)
      { // if within view angle
        if (!Physics.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, DOOR_OBSTRUCTION_MASK))
        { // if no walls
          Debug.DrawRay(transform.position, directionToPlayer, Color.red);
//          Debug.Log("Player seen");
          canSeePlayer = true;
          // update player seen
          playerSeen.lastSeenHeard = Time.time;
          playerSeen.lastPosition = player.position;
          // Steer towards where the player will be

          float angleRelativeHeading = Vector3.Angle(transform.forward.normalized, playerRb.velocity.normalized);
          // Debug.Log($"angle RH: {angleRelativeHeading}, ");
          // if facing each other, use seek algorithm
          if (angleRelativeHeading < 45.0f || angleRelativeHeading > 135.0f || directionToPlayer.magnitude < 1.0f)
          {
            // heading towards us

            target = player.position;
            // Debug.Log($"E_{enemyId}: target player {target}, ");

          }
          else
          {
            // pursuit behaviour
            float lookAheadTime = directionToPlayer.magnitude / (agent.speed + playerRb.velocity.magnitude);
            Vector3 preTarget = player.position + (playerRb.velocity.normalized * (playerRb.velocity.magnitude * lookAheadTime));
            NavMeshHit closestWalkable;
            if (NavMesh.SamplePosition(preTarget, out closestWalkable, 6.0f, 1))
            {
              target = closestWalkable.position;
            }
            else
            {
              target = playerSeen.lastPosition;
            }
//            Debug.Log($"E_{enemyId}: target future {target}");
          }
        }
        else
        {
          canSeePlayer = false;
          // Debug.Log($"EC@FOVC: E_{enemyId} player not detected (behind wall)");
        }
      }
      else
      {
        canSeePlayer = false;
      }
    }
    else
    {
      canSeePlayer = false;
    }
  }

  void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(target, 1.0f);
    // Gizmos.color = Color.red;
    // Gizmos.DrawWireSphere(transform.forward, 1.0f);
    // Gizmos.color = Color.blue;
    // Gizmos.DrawWireSphere(agent.transform.forward, 1.0f);
  }
  public float DistToLastPlayerPos()
  {
    return (transform.position - playerSeen.lastPosition).magnitude;
  }
  public float DistToTarget()
  {
    return (agent.transform.position - target).magnitude;
  }
}