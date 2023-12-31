using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Transform PlayerCharacter;
    [Header("MaxDistance you can open or close the door.")]
    public float MaxDistance = 5;

    DoorState tempObject;


    

    void Update()
    {
        //This will tell if the player press F on the Keyboard. P.S. You can change the key if you want.
        if (Input.GetKeyDown(KeyCode.F))
        {
            Pressed();
            //Delete if you dont want Text in the Console saying that You Press F.
            //Debug.Log("You Press F");
        }
    }

    void Pressed()
    {
        //This will name the Raycasthit and came information of which object the raycast hit.
        RaycastHit doorhit;

        if (Physics.Raycast(PlayerCharacter.transform.position, PlayerCharacter.transform.forward, out doorhit, MaxDistance))
        {

            // if raycast hits, then it checks if it hit an object with the tag Door.
            if (doorhit.transform.tag == "Door")
            {
                tempObject = doorhit.collider.GetComponent<DoorState>();
                tempObject.SendMessage("DoorHit");
                
            }
        }
    }
}