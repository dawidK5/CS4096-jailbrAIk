using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogEnemyRef : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public GameObject playerRef;
    public PlayerController player;

    [Header("Stats")]
    public float pathUpdateDelay = 0.2f;

    public void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
         

    }
    
}
