using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimController : MonoBehaviour
{
    public Animator animator;
    public MyPlayerMovement MPM;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        if (Input.GetButton("Move_Buttons") || Input.GetButton("Move_Arrows"))
            animator.SetFloat("Speed", 1);

        if (!Input.GetButton("Move_Buttons") && !Input.GetButton("Move_Arrows"))
            animator.SetFloat("Speed", 0);
        #endregion

        #region Actions
        if (Input.GetButtonDown("Dash") && MPM.canDash)
        {
            animator.SetTrigger("Dash");
            StartCoroutine(MPM.Dash()); 
        }

        if (Input.GetButtonDown("Slide") && MPM.canDash && MPM.IsGrounded())
        {
            animator.SetTrigger("Slide");
            StartCoroutine(MPM.Dash()); 
        }

        if (Input.GetButton("Jump") && MPM.IsGrounded())
            animator.SetTrigger("Jump");

        if (MPM.IsGrounded())
            animator.SetBool("Fall", false);
        else
            animator.SetBool("Fall", true);
        #endregion
    }
}
