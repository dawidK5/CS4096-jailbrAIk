using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
  [SerializeField]
  public Transform playerRef;
  public float maxDist = 2.0f;
  public bool inTrigger = false;
  private PickupScreen ps;
  private RaycastHit hit;

  // void Update()
  // {
  //   if (Input.GetKeyDown(KeyCode.J))
  //   {
  //     Pressed();
  //   }
  // }

  public bool FacingObject()
  {
    Debug.DrawRay(playerRef.transform.position, playerRef.transform.forward, Color.red);

    if (Physics.Raycast(playerRef.transform.position, playerRef.transform.forward, out hit, maxDist, 1<<11))
    {
      Debug.Log($"Item: raycast tag: {hit.transform.tag}");
      if (hit.transform.tag == "Item")
      {
        return true;
        // ps = hit.collider.GetComponent<PickupScreen>();
        // ps.SendMessage("Pickup", hit.collider.name);
      }
    }
    Debug.Log("Item: no raycast");
    return false;
  }
  public void OnTriggerEnter(Collider other)
  {
    Debug.Log($"ItemScript@OTen: collider {other.tag}");
    if (other.CompareTag("Item"))
    {
      Debug.Log("Player entered item trigger");
      inTrigger = true;
      StartCoroutine("CheckIfPressedAndLooking");
      // while(inTrigger)
      // {
      //   if (Input.GetKeyDown(KeyCode.J))
      //   {
      //     Debug.Log("ItemScript@OTEn: J pressed");
      //     if (FacingObject())
      //     {
      //       Debug.Log("Item picked up");
      //       break;
          
      //     }
      //   }
      // }
    }
  }

  IEnumerator CheckIfPressedAndLooking()
  {
    WaitForFixedUpdate wait = new WaitForFixedUpdate();
    while (true)
    {
      yield return wait;
      if (Input.GetKeyDown(KeyCode.J))
      {
        Debug.Log("ItemScript@OTEn: J pressed");
        if (FacingObject())
        {
          Debug.Log("Item picked up");
          StopCoroutine("CheckIfPressedAndLooking");
        }
      }
    }
  }
    
  // public void OnTriggerExit(Collider other)
  // {
  //   inTrigger = false;
  //   Debug.Log("Item trigger left");
  // }
}