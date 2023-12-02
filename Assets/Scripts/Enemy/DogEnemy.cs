using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlay;

public class DogEnemy : MonoBehaviour, IHear
{

    public Transform target;

    private DogEnemyRef dogEnemyRef;

    private float shootingDistance;

    private float pathUpdateDeadline;
    public FieldOfView fieldOfView;

    //public GameManager gameManager;

    //DogAgent dogAgent;

    [SerializeField] private AudioSource source;

    [SerializeField] private float soundRange = 25f;

    [SerializeField] private Sound.SoundType soundType = Sound.SoundType.Dangerous;

    public DogPatrol dogPatrol;

    private float elapsedTime = 0.0f;
    DogBT dogBT;
    private float dogBoredOrTired = 0.0f;


    private void Awake()
    {
        dogEnemyRef = GetComponent<DogEnemyRef>();
    }
    // Start is called before the first frame update
    void Start()
    {
        shootingDistance = dogEnemyRef.navMeshAgent.stoppingDistance;
        //GameObject dogEnemyGameObject = GameObject.FindGameObjectWithTag("DogEnemy");
        fieldOfView = gameObject.GetComponent<FieldOfView>();
        //dogAgent = GetComponent<DogAgent>();
        //foreach (Transform child in gameObject.transform)
        //{
        //    // Do something with the child Transform
        //    if (child.name == "DogPatrol")
        //    {
        //        print("yay");
        //        dogPatrol = GetComponentInChildren<DogPatrol>();
        //    }
        //}


    }

    // Update is called once per frame
    void Update()
    {

        if (fieldOfView.canSmellPlayer)
        {
            bool inRange = Vector3.Distance(transform.position, fieldOfView.lastSmelledPosition) <= shootingDistance;
            if (inRange)
            {
                LookAtTarget();
            }
            else
            {
                updatePath(fieldOfView.lastSmelledPosition);
                //dogAgent.dogStateMachine.ChangeState(DogStateId.ChasePlayer);
            }

        }
        if (fieldOfView.canSeePlayer)
        {
            bool inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;
            if (inRange)
            {
                LookAtTarget();
            }
            else
            {
                updatePath(target.position);
            }

        }

        //if ((target.position - gameObject.transform.position).sqrMagnitude < 2.0f)
        //{
        //    gameManager.gameOverScreen.Setup();
        //    //target.gameObject.active = false;
        //}

        if (!fieldOfView.canSmellPlayer && !fieldOfView.canSeePlayer && fieldOfView.playerLastLocatedTime < Time.time - 10f)
        {
            //Debug.Log("dog bored or tired");

            //Debug.Log("dog bored or tired");
            dogBoredOrTired = 0.0f;
            dogPatrol = (DogPatrol)gameObject.GetComponent<DogBT>().root;
            dogPatrol.UpdateDestination();

        }
        // else
        // {
        //     dogBoredOrTired = 0.0f;
        // }
        // {
        // if (elapsedTime > 3.0f)
        // {
        //     Debug.Log("dog not patrol null");
        //     dogPatrol = (DogPatrol)gameObject.GetComponent<DogBT>().root;
        //     dogPatrol.UpdateDestination();
        //     elapsedTime = 0.0f;
        // }
        // elapsedTime += Time.deltaTime;
    // }

    //}
    //$$$dogEnemyRef.animator.SetFloat("speed", dogEnemyRef.navMeshAgent.desiredVelocity.sqrMagnitude);
}

public void LookAtTarget()
{
    Vector3 lookPos = target.position - transform.position;
    lookPos.y = 0;
    Quaternion rotation = Quaternion.LookRotation(lookPos);
    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
}

public void updatePath(Vector3 position)
{
    if (Time.time >= pathUpdateDeadline)
    {
        if (!source.isPlaying) //If already playing a sound, don't allow overlapping sounds 
        {
            source.Play();
        }
        var sound = new Sound(transform.position, soundRange, soundType); // $$$$change this . property

        Sounds.MakeSound(sound);

        //Debug.Log("Updating Path");
        pathUpdateDeadline = Time.time + dogEnemyRef.pathUpdateDelay;
        dogEnemyRef.navMeshAgent.SetDestination(position);
    }

}

public void RespondToSound(Sound sound)
{
    if (gameObject.transform.position != sound.pos)
    { // priority
      //Debug.Log(name + "responding to sound at" + sound.pos);
        dogEnemyRef.navMeshAgent.SetDestination(sound.pos);
    }
}


    //public void OnTriggerEnter(Collider col)
    //{
    //    if (col.gameObject.layer == 8) { 
    //       Debug.Log("collision with " + col.gameObject.name);
    //    }
    //}
}
