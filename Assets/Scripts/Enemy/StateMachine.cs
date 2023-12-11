using UnityEngine;
using STMGR;

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
    new ChaseState(),
    new LostChase()
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
    if (loopTime > 0.125f)
    {
      if (fsmStatus.transitionDue)
      {
        ChangeState(fsmStatus.nextState);
      }
      else
      {
        allStates[(int)currentState].UpdateState(loopTime);
//        Debug.Log($"Executed in FSM: {loopTime}");
        loopTime = 0.0f;
      }
    }
    loopTime += Time.deltaTime;
  }

  private void ChangeState(ENEMY_STATES nextState)
  {
    fsmStatus.transitionDue = false;
    allStates[(int)currentState].ExitState();
    currentState = nextState;
//    Debug.Log($"Change state to {nextState}");
    this.enemy.currentState = nextState;
    allStates[(int)currentState].EnterState(fsmStatus);
  }
}