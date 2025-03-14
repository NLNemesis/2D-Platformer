using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimController : MonoBehaviour
{
    #region Variables
    private Animator animator;
    private MyPlayerMovement MPM;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        MPM = GetComponent<MyPlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        if (Input.GetButton("Movement_Buttons") || Input.GetButton("Movement_Arrows"))
            animator.SetFloat("Speed", 1);

        if (!Input.GetButton("Movement_Buttons") && !Input.GetButton("Movement_Arrows"))
            animator.SetFloat("Speed", 0);
        #endregion

        #region Actions
        if (Input.GetButtonDown("Jump"))
            animator.SetTrigger("Jump");
        #endregion
    
        if (MPM.IsGrounded())
            animator.SetTrigger("Fall");
        else
            animator.ResetTrigger("Fall");
    }
}
