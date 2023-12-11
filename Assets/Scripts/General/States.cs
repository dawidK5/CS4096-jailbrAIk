using STMGR;
using UnityEngine;

public abstract class State
{
  public EnemyController enemy;
  public FSMStatus fsmStatus;
  protected float updateInterval = 0.25f; 
  public abstract void EnterState(FSMStatus fsmStatus);
  public abstract void UpdateState(float deltaTime);
  public abstract void ExitState();
}


public sealed class IdleState : State
{
  private float[] waitTimes = new float[2]{
    8.0f, 1.0f
  };
  public override void EnterState(FSMStatus fsmStatus)
  {
    this.fsmStatus = fsmStatus;
    enemy.deltaBored = 0.1f;
    enemy.deltaTired = 0.0f;
    enemy.agent.speed = enemy.DEFAULT_SPEED;
    enemy.agent.acceleration = enemy.DEFAULT_ACCELERATION;
    // Debug.Log($"FSM@EnS: E_{enemy.enemyId} entered idle");
  }

  public override void UpdateState(float deltaTime)
  {
    if (enemy.canSeePlayer)
    {
      // Debug.Log("Saw player, will begin chasing");
      fsmStatus.nextState = ENEMY_STATES.CHASE;
      fsmStatus.transitionDue = true;
      return;
    }
    if (enemy.bored < 0.0f)
    {
      // Debug.Log("Got bored, will begin patrolling");
      fsmStatus.nextState = ENEMY_STATES.PATROL;
      fsmStatus.transitionDue = true;
      return;
    }
    waitTimes[0] -= deltaTime;
    if (waitTimes[0] > 0.0f)
    {
//      Debug.Log($"Waiting idle {waitTimes[0]}");
      return;
    }
    else
    {
      // Debug.Log("Finished idle waiting, will patrol");
      fsmStatus.nextState = ENEMY_STATES.PATROL;
      fsmStatus.transitionDue = true;
    }
  }

  public override void ExitState()
  {
    enemy.bored = 1.0f;
    enemy.tired = 1.0f;
    // Debug.Log($"FSM@ExS: E_{enemy.enemyId} exited idle");
  }
}

public sealed class PatrolState : State
// sealed because we wont subclass further, tag for performance
{
  private EnemyPatrol patrol;
  
  public override void EnterState(FSMStatus fsmStatus)
  {
    this.fsmStatus = fsmStatus;
    enemy.deltaBored = 0.02f;
    enemy.deltaTired = 0.0f;
    enemy.agent.speed = enemy.DEFAULT_SPEED;
    enemy.agent.acceleration = enemy.DEFAULT_ACCELERATION;
    patrol = enemy.patrol;
    patrol.enemy = this.enemy;
    // Debug.Log($"FSM@EnS: E_{enemy.enemyId} entered patrol state");
    patrol.Setup(fsmStatus, enemy);
  }

  public override void UpdateState(float deltaTime)
  {
    patrol.RunUpdate(deltaTime);
  }

  public override void ExitState()
  {
    patrol.Reset();
    enemy.bored = 1.0f;
    enemy.tired = 1.0f;
    // Debug.Log($"FSM@ExS: E_{enemy.enemyId} exited patrol state");
  }


}

public sealed class ChaseState : State
{
  private float timeWhenLastSeen = 0.0f;
  private float runningTimeSinceSeen = 0.0f;
  private float maxChaseSpeed;
  public override void EnterState(FSMStatus fsmStatus)
  {
    this.fsmStatus = fsmStatus;
    // Debug.Log($"FSM@EnS:  E_{enemy.enemyId} entered chasing");

    maxChaseSpeed = enemy.DEFAULT_SPEED * 1.5f;
    runningTimeSinceSeen = 0.0f;
    enemy.agent.speed = maxChaseSpeed;
    enemy.agent.acceleration = enemy.DEFAULT_ACCELERATION *1.5f;
    enemy.bored = 0.0f;
    enemy.deltaBored = 0.0f;
    enemy.deltaTired = 0.005f;

    enemy.agent.SetDestination(enemy.target);
    // enemy.agent.SetDestination(enemy.playerSeen.lastPosition);
  }

  public override void UpdateState(float deltaTime)
  {
    enemy.agent.SetDestination(enemy.target);
    enemy.agent.isStopped = false;

    if (enemy.tired < 0.001f)
    {
      // Debug.Log($"FSM@US: E_{enemy.enemyId} tired, stop chaise, wil look around");
      enemy.agent.ResetPath();
      enemy.agent.isStopped = true;
      fsmStatus.nextState = ENEMY_STATES.LOST_CHASE;
      fsmStatus.transitionDue = true;
      return;
    }
    if (!enemy.canSeePlayer)
    { // if cannot see player
      runningTimeSinceSeen += deltaTime;
    }
    else
    {
      runningTimeSinceSeen = 0.0f;
    }
    if (runningTimeSinceSeen > 10.0f)
    {
      // Debug.Log($"FSM@US: E_{enemy.enemyId} lost player for 10s, will look around");
      fsmStatus.nextState = ENEMY_STATES.LOST_CHASE;
      fsmStatus.transitionDue = true;
      return;
    }
  }

  public override void ExitState()
  {
    // Debug.Log($"FSM@ExS:  E_{enemy.enemyId} exited chasing");
    enemy.tired = 1.0f;
    enemy.agent.ResetPath();
    enemy.agent.isStopped= true;
  }
}

public sealed class LostChase : State
{
  private bool[] rotationDone = new bool[3]{false, false, false};
  private static readonly float[] ROTATIONS = new float[3]{-90.0f, 180.0f, -90.0f};
  private int step = 0;

  private void ResetRotations()
  {
    for (int i = 0; i < 3; i++)
    {
      rotationDone[i] = false;
    }
  }

  public override void EnterState(FSMStatus fsmStatus)
  {
    this.fsmStatus = fsmStatus;
    step = 0;
    enemy.agent.isStopped = false;
    enemy.agent.SetDestination(enemy.target);
    enemy.agent.speed = enemy.DEFAULT_SPEED;
    enemy.agent.acceleration = enemy.DEFAULT_ACCELERATION;
    ResetRotations();
    enemy.deltaBored = 0.02f;
    // Debug.Log($"FSM@ExS:  E_{enemy.enemyId} entered lost chase");

  }

  private bool RotationsDone()
  { // returns true when all rotations
    for (int i = 0 ; i < 3; i++)
    {
      if(!rotationDone[i])
      {
        if (enemy.isRotating)
        {
          return false;
        }
        // Debug.Log($"E_{enemy.enemyId}: Starts {i+1}th rotation");
        enemy.RotateTo(ROTATIONS[i], 1.5f);
        rotationDone[i] = true;
        return false;
      }
    }
    // Debug.Log("Finished lost chase rotations");
    return true;
  }
  public override void UpdateState(float deltaTime)
  {
    if (enemy.canSeePlayer)
    {
      fsmStatus.nextState = ENEMY_STATES.CHASE;
      fsmStatus.transitionDue = true;
      return;
    }
    if (enemy.DistToTarget() < 2.5f)
    { // when reached last pos, look around
      enemy.agent.ResetPath();
      enemy.agent.isStopped = true;
      
      if (!RotationsDone())
      {
        return;
      }
      if (enemy.bored < 0.3f)
      {
        // Debug.Log($"E_{enemy.enemyId}@LOST_CHASE: Finished looking, will patrol");
        fsmStatus.nextState = ENEMY_STATES.PATROL;
        fsmStatus.transitionDue = true;
      }
    }
  }

  public override void ExitState()
  {
    enemy.agent.ResetPath();
    enemy.agent.isStopped = true;
    enemy.bored = 1.0f;
  }
}