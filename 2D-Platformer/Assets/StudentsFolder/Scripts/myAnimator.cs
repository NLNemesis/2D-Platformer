using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myAnimator : MonoBehaviour
{
    public Animator animator;
    public myPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.inAir == false)
        {
            if (Input.GetButton("Move_Buttons") || Input.GetButton("Move_Arrows"))
                animator.Play("Move");
            else
                animator.Play("Idle");

            if (Input.GetButtonDown("Jump"))
            {
                player.inAir = true;
                animator.Play("Jump");
            }
        }
    }
}
