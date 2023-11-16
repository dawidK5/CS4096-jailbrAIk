using UnityEngine;
using UnityEditor;
using System;
public class PatrolNode : MonoBehaviour
{
 
  [SerializeField]
  public float nearNodesRadiusSq = 210.0f;  // square of the value
  public PatrolNode[] nearNodes = new PatrolNode[MAX_SLOTS];
  public Vector3 location; // duplicated for ease of access
  public int nearNodesIndex = 0;
  public float radius = 0.5f;
  const int MAX_SLOTS = 9;
  public void Awake()
  {
    location = this.GetComponent<Transform>().position;
    nearNodes = new PatrolNode[9];
  }

  // public void Start()
  // {
  //   Debug.Log("List " + nearNodes);
  //   // Debug.Log($"Dist is {Vector3.Distance(new Vector3(19,0,6), new Vector3(27,0,-4))}");
  // }

  // public virtual void OnDrawGizmos()
  // {
  //   Gizmos.color = Color.red;
  //   Gizmos.DrawWireDisc(transform.position, 14.49f);
  // }

  private void OnDrawGizmos()
  {
    Handles.color = Color.red;
    Handles.DrawWireDisc(location, Vector3.up, 14.49f);
  }

  public void AddNear(PatrolNode pn)
  {
    if (nearNodesIndex < MAX_SLOTS)
    {
      Debug.Log("Adding at " + nearNodesIndex + " to " + this.ToString());
      nearNodes[nearNodesIndex] = pn;
      nearNodesIndex++;
    }
  }
}
