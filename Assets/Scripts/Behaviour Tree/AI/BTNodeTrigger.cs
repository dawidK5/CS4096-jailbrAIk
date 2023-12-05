using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BTNodeTrigger : MonoBehaviour
{
    private static int _playerLayerMask = 3;
    public bool playerInTrigger = false;
    public float lastExit = 0f;
    public List<GameObject> enemiesInRange = new List<GameObject>();
    public Transform _transform;
    public int alertRadius = 200;
    public bool alertState = false;
    public float timeToDisengage = 3f;
    public AudioSource audioSource;
    public Vector3 lastPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger == false && lastExit < Time.fixedTime - timeToDisengage)
        {
            enemiesInRange.Clear();
            alertState = false;
        }
        if (alertState)
        {
            
            foreach(GameObject enemy in enemiesInRange)
            {
                Alertable alertScript = enemy.GetComponent<Alertable>();
                if (alertScript != null)
                {
                    if (playerInTrigger == true)
                    {
                        enemy.GetComponent<Alertable>().SendMessage("Alert", transform.position);
                    }
                    else
                    {
                        enemy.GetComponent<Alertable>().SendMessage("Alert", lastPlayerPosition);
                    }

                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == _playerLayerMask)
        {
            if (!audioSource.isPlaying) 
            { 
                audioSource.Play();
            }
            playerInTrigger = true;
            alertState = true;
            Collider[] colliders = Physics.OverlapSphere(transform.position, alertRadius);

            
            foreach (Collider hit in colliders)
            {
               
                if (hit.gameObject.tag == "Enemy")
                {

                    enemiesInRange.Add(hit.gameObject);

                }
                
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerLayerMask)
        {
            lastExit = Time.fixedTime;
            playerInTrigger = false;
            lastPlayerPosition = other.gameObject.transform.position;
        }
    }
}
