using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolNode : MonoBehaviour
{
 
  [SerializeField]
  public float nearNodesRadius = 210.0f;  // square of the value
  public PatrolNode[] nearNodes;
  public Vector3 location; // duplicated for ease of access
  public int nearNodesIndex = 0;
  public float radius = 0.5f;
  
  public void Awake()
  {
    location = this.GetComponent<Transform>().position;
    nearNodes = new PatrolNode[9];
  }

  public void Start()
  {
    Debug.Log("List " + nearNodes);
    // Debug.Log($"Dist is {Vector3.Distance(new Vector3(19,0,6), new Vector3(27,0,-4))}");
  }

  public virtual void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawSphere(transform.position, radius);
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, nearNodesRadius);
  }

  public void AddNear(PatrolNode pn)
  {
    nearNodes[nearNodesIndex] = pn;
    nearNodesIndex++;
  }
}
