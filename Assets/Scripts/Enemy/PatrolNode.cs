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
    public float nearNodesRadius = 50.0f;
    public Vector3 location;
    public List<PatrolNode> nearestNodes;

    // public void Start()
    // {
    //   // get nearest nodes as children of the parent GameObject

    // }
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }


}
