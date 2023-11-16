using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
  [SerializeField]
  public Transform playerRef;
  public float maxDist = 2.0f;
  
  private PickupScreen ps;
  private RaycastHit hit;

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.J))
    {
      Pressed();
    }
  }

  void Pressed()
  {
    if (Physics.Raycast(playerRef.transform.position, playerRef.transform.forward, out hit, maxDist))
    {
      if (hit.transform.tag == "Item")
      {
        ps = hit.collider.GetComponent<PickupScreen>();
        ps.SendMessage("Pickup", hit.collider.name);
      }
    }
  }
}