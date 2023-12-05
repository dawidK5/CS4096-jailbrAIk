using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STMGR;
using UnityEngine.Assertions.Must;

public class StateMachine : MonoBehaviour
{
  [SerializeField]
  float loopTime = 0.0f;
  ENEMY_STATES currentState = ENEMY_STATES.IDLE;
  public FSMStatus fsmStatus = new FSMStatus
  {
    sensingFinished = true,
    updateFinished = true,
    transitionDue = false,
    nextState = ENEMY_STATES.IDLE,
  };

  private readonly State[] allStates = new State[(int)ENEMY_STATES.NUM_STATES]
  {
    new IdleState(),
    new PatrolState(),
    new ChaseState()
  };

  [SerializeField]
  EnemyController enemy;

  // Start is called before the first frame update
  void Awake()
  {
    foreach (State st in allStates)
    {
      st.enemy = this.enemy;
    }
    currentState = ENEMY_STATES.IDLE;
  }
  void Start()
  {
    allStates[(int)currentState].EnterState(fsmStatus);
  }

  void Update()
  {
    if (loopTime > 0.25f)
    {
      if (fsmStatus.transitionDue)
      {
        ChangeState(fsmStatus.nextState);
      }
      else
      {
        allStates[(int)currentState].UpdateState(loopTime);
        Debug.Log($"Executed in FSM: {loopTime}");
        loopTime = 0.0f;
      }
    }
    loopTime += Time.deltaTime;
  }
  // Update is called once per frame
  // void Update()
  // {
  //   if (loopTime > 0.25f)
  //   {
  //     if (fsmStatus.transitionDue)
  //     {
  //       fsmStatus.transitionDue = false;
  //       ChangeState(fsmStatus.nextState);
  //     }

  //     // if (fsmStatus.updateFinished && fsmStatus.sensingFinished && !updateCoroutineRunning)
  //     // {
  //     //   Debug.Log("Start Coroutine");
  //     //   fsmStatus.updateFinished = false;
  //     //   updateCoroutineRunning = true;
  //     //   StartCoroutine(allStates[(int)currentState].UpdateState(fsmStatus));
  //     // }
  //     // else
  //     // {
  //     //   Debug.Log("Update or sense not finished");
  //     // }
  //     loopTime = 0.0f;
  //   }
  //   loopTime += Time.deltaTime;
  //   // bored -= 0.1f;
  // }

  private void ChangeState(ENEMY_STATES nextState)
  {
    fsmStatus.transitionDue = false;
    allStates[(int)currentState].ExitState();
    currentState = nextState;
    allStates[(int)currentState].EnterState(fsmStatus);
  }
  // state machine should call UpdateState (and do the clocking), Update state should check and return
  // pass delta time to UpdateState
  // ---- pass 0 do logic add 5 
}