using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public abstract class State
{
    protected NavMeshAgent agent;
    protected Transform player;
    public virtual void exec() { }
}

public sealed class IdleState : State
{
    public override void exec()
    {
        Debug.Log("Enemy is idling");
    }
}

public sealed class PatrolState : State
// sealed because we wont subclass further, tag for performance
{
  EnemyPatrol patrol;
    // private float changeRoute = 1.0f;
    // public PatrolState(NavMeshAgent agent_ptr, Transform player_ptr)
    // {
    //     agent = agent_ptr;
    //     playerState = player_ptr;
    //     NAME = "PATROL";
    //     // find game objects with tag Node
    //     GameObject[] spheres = GameObject.FindGameObjectsWithTag("Node");
    //     for (int i = 0; i < 12; i++)
    //     {
    //         NODES[i] = spheres[i].transform.position;
    //         Debug.Log("Sucess - spheres found");
    //         break;
    //         // new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f))
    //     }

    public override void exec()
    {
      Debug.Log("Enemy has entered the patrolling state");
      
    }
}

public sealed class ThinkingState : State
{
  public override void exec()
  {
    Debug.Log("Agent is thinking");
  }
}

public sealed class ChaseState : State
{
  public override void exec()
  {
    Debug.Log("Agent is chaising");
  }
}

public sealed class AttackState : State
{
  public override void exec()
  {
    Debug.Log("Agent is attacking");
  }
}