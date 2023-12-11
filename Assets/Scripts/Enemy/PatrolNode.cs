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
    nodeId = Utils.getId(name);
    
  }

  // private void OnDrawGizmos()
  // {
  //   Handles.color = Color.red;
  //   Handles.DrawWireDisc(location, Vector3.up, radius);
  // }

  public void AddNear(PatrolNode pn)
  {
    if (nearNodesIndex < MAX_SLOTS)
    {
      // Debug.Log("Adding at " + nearNodesIndex + " to " + this.ToString());
      nearNodes.Add(pn);
      nearNodesIndex++;
    }
  }
}
