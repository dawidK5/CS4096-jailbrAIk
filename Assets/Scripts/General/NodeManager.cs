using UnityEngine;

public class NodeManager : MonoBehaviour
{
  private static PatrolNode[] pNodes;

  public static void AssignNearestNeighbours()
  {
    for (int i = 0; i < pNodes.Length; i++)
    {
      for (int j = 0; j < pNodes.Length; j++)
      {
        if (i != j)
        {
          float dist = (pNodes[i].location - pNodes[j].location).sqrMagnitude;
          // Debug.Log("Dist " + i + " to " + j + " " + dist);
          if (dist < pNodes[i].nearNodesRadiusSq)
          {
            // Debug.Log("Attempt: add " + j + " to " + i);
            pNodes[i].AddNear(pNodes[j]);
          }
        }
      }
    }
  }

  void Start()
  {
    pNodes = GetComponentsInChildren<PatrolNode>();
    AssignNearestNeighbours();
    Debug.Log("NM@St: Node neighbours assigned");
  }
}
