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
    private Sound sound;

    private float lastBarkTime = 0.0f;

    private void Awake()
    {
        dogEnemyRef = GetComponent<DogEnemyRef>();
    }
    // Start is called before the first frame update
    void Start()
    {
        shootingDistance = dogEnemyRef.navMeshAgent.stoppingDistance;
        fieldOfView = gameObject.GetComponent<FieldOfView>();
        sound = new Sound(Vector3.zero, soundRange, soundType);
    }

    // Update is called once per frame
    void Update()
    {
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
            }

        }
       

        if (!fieldOfView.canSmellPlayer && !fieldOfView.canSeePlayer && fieldOfView.playerLastLocatedTime < Time.time - 10f)
        {
            
            dogBoredOrTired = 0.0f;
            dogPatrol = (DogPatrol)gameObject.GetComponent<DogBT>().root;
            dogPatrol.UpdateDestination();

        }
        
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
        if (!source.isPlaying && ((Time.time - lastBarkTime) > 4f || lastBarkTime == 0.0f)) //If already playing a sound, and if barked in the last 4 seconds, don't allow overlapping sounds 
        {
            source.Play();
            lastBarkTime = Time.time;
        }
        // var sound = new Sound(transform.position, soundRange, soundType); // $$$$change this . property
        sound.pos = transform.position;
        Sounds.MakeSound(sound);

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


}
