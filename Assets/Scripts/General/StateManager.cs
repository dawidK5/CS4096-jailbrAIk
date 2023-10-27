namespace STMGR
{
    public enum ENEMY_STATES
    {
        IDLE,
        PATROL,
        // THINK,
        CHASE,
        // ATTACK,
        NUM_STATES
  };
  public class FSMStatus
  {
    
    public bool sensingFinished;
    public bool updateFinished;
    public bool transitionDue;
    public ENEMY_STATES nextState;
  };
}