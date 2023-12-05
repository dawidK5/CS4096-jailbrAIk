using STMGR;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.AI;

public abstract class State
{
  // public GameManager gm = GameManager.Instance();
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
    3.0f, 1.0f
  };
  public override void EnterState(FSMStatus fsmStatus)
  {
    this.fsmStatus = fsmStatus;
    enemy.agent.speed = enemy.DEFAULT_SPEED;
    Debug.Log($"FSM@EnS: E_{enemy.enemyId} entered idle");
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
      Debug.Log($"Waiting idle {waitTimes[0]}");
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
    Debug.Log($"FSM@ExS: E_{enemy.enemyId} exited idle");
  }
}

public sealed class PatrolState : State
// sealed because we wont subclass further, tag for performance
{
  private EnemyPatrol patrol;

  public override void EnterState(FSMStatus fsmStatus)
  {
    this.fsmStatus = fsmStatus;
    enemy.agent.speed = enemy.DEFAULT_SPEED;
    patrol = enemy.patrol;
    Debug.Log($"FSM@EnS: E_{enemy.enemyId} entered patrol state");
    patrol.Setup(fsmStatus);
  }
  public override void UpdateState(float deltaTime)
  {
    patrol.RunUpdate(deltaTime);
  }

  public override void ExitState()
  {
    patrol.Reset();
    enemy.bored = 1.0f;
    Debug.Log($"FSM@ExS: E_{enemy.enemyId} exited patrol state");
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
    Debug.Log($"FSM@EnS:  E_{enemy.enemyId} entered chasing");
    maxChaseSpeed = enemy.DEFAULT_SPEED * 4.0f;
    enemy.agent.speed = maxChaseSpeed;
    enemy.agent.SetDestination(enemy.playerSeen.lastPosition);
    timeWhenLastSeen = 0.0f; // enemy.playerSeen.lastSeenHeard;
  }

  public override void UpdateState(float deltaTime)
  {
    if (timeWhenLastSeen < enemy.playerSeen.lastSeenHeard)
    {
      timeWhenLastSeen = enemy.playerSeen.lastSeenHeard;
      runningTimeSinceSeen = 0.0f;
    }
    // if(enemy.canCapturePlayer)
    // {
    //   //GameManager.StopGame();
    // }
    if (enemy.tired < 0.1f)
    {
      Debug.Log($"FSM@US: E_{enemy.enemyId} tired, stoppped chaising");
      enemy.agent.ResetPath();
      fsmStatus.nextState = ENEMY_STATES.IDLE;
      fsmStatus.transitionDue = true;
      return;
    }
    if (enemy.agent.remainingDistance == 0.0f)
    { // if enemy is at last player pos
      runningTimeSinceSeen += deltaTime;
    }
    if (runningTimeSinceSeen > 5.0f)
    {
      Debug.Log($"FSM@US: E_{enemy.enemyId} stops chase aft 5s at last player pos, will patrol");
      fsmStatus.nextState = ENEMY_STATES.PATROL;
      fsmStatus.transitionDue = true;
      return;
    }
    enemy.agent.speed = maxChaseSpeed * enemy.tired;
    enemy.agent.SetDestination(enemy.playerSeen.lastPosition);
    enemy.tired -= 0.02f;
  }

  public override void ExitState()
  {
    Debug.Log($"FSM@ExS:  E_{enemy.enemyId} exited chasing");
    enemy.tired = 1.0f;
  }
}