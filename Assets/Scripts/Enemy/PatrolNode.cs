using UnityEngine;

public class PatrolNode : MonoBehaviour
{
 
  [SerializeField]
  public float nearNodesRadiusSq = 210.0f;  // square of the value
  public PatrolNode[] nearNodes = new PatrolNode[9];
  public Vector3 location; // duplicated for ease of access
  public int nearNodesIndex = 0;
  public float radius = 0.5f;
  
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

  public virtual void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, 14.49f);
  }

  public void AddNear(PatrolNode pn)
  {
    Debug.Log("Adding at " + nearNodesIndex + " to " + this.ToString());
    nearNodes[nearNodesIndex] = pn;
    nearNodesIndex++;
  }
}
