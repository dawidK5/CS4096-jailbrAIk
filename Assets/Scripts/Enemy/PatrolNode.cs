using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
public class PatrolNode : MonoBehaviour
{
 
  [SerializeField]
  public float nearNodesRadiusSq;  // square of the value
  public List<PatrolNode> nearNodes;
  public Vector3 location; // duplicated for ease of access
  public int nearNodesIndex = 0;
  public float radius;
  public int nodeId;
  const int MAX_SLOTS = 9;
  public bool occupied = false;
  
  

  public void Awake()
  {
    location = this.GetComponent<Transform>().position;
    nearNodes = new List<PatrolNode>(MAX_SLOTS);
    // nearNodesRadiusSq = radius * radius;
    // repeated for MonoBehaviour handling and avoiding index errors
    nodeId = Utils.getId(name);
    
  }

  // public void Start()
  // {
  //   Debug.Log("List " + nearNodes);
  //   Debug.Log($"Dist is {Vector3.Distance(new Vector3(19,0,6), new Vector3(27,0,-4))}");
  // }

  // public virtual void OnDrawGizmos()
  // {
  //   Gizmos.color = Color.red;
  //   Gizmos.DrawWireDisc(transform.position, 14.49f);
  // }

  private void OnDrawGizmos()
  {
    Handles.color = Color.red;
    Handles.DrawWireDisc(location, Vector3.up, radius);
  }

  public void AddNear(PatrolNode pn)
  {
    if (nearNodesIndex < MAX_SLOTS)
    {
      Debug.Log("Adding at " + nearNodesIndex + " to " + this.ToString());
      // nearNodes[nearNodesIndex] = pn;
      nearNodes.Add(pn);
      nearNodesIndex++;
    }
  }
}
