using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorState : MonoBehaviour
{
    private Animator anim;

    public bool open = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
 

    public void DoorHit()
    {
        anim = GetComponentInParent<Animator>();
        //This line will set the bool true so it will play the animation.
        anim.SetBool("Opened", !open);
        open = !open;
    }

    public void Open()
    {
        anim = GetComponentInParent<Animator>();
        //This line will set the bool true so it will play the animation.
        anim.SetBool("Opened", true);
        open = true;
    }

    public void Close()
    {
        anim = GetComponentInParent<Animator>();
        //This line will set the bool true so it will play the animation.
        anim.SetBool("Opened", false);
        open = false;
    }
}
