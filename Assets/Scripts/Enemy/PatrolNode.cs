using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolNode : MonoBehaviour
{
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    [SerializeField]
    public float radius = 0.5f;
    public float nearNodesRadius = 10.0f;
    public Vector3 location; // duplicated for ease of access
    public List<PatrolNode> nearNodes;

    public void Start()
    {
      location = transform.position;
      // Debug.Log($"Dist is {Vector3.Distance(new Vector3(19,0,6), new Vector3(27,0,-4))}");
    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nearNodesRadius);
    }


}
