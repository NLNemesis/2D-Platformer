using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimController : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
    public bool HasDash;
    public bool HasSlide;
    private bool CanAttack = true;
    private float Speed;

    [Header("Attack Controller")]
    public int MaxAttack;
    private int CurrentAttack;
    private bool CanCombo;
    private bool ComboActivated;

    [Header("References")]
    private Animator animator;
    private MyPlayerMovement MPM;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        MPM = GetComponent<MyPlayerMovement>();
        Speed = MPM.speed;
    }

    // Update is called once per frame
    void Update()
    {
        //if (MPM.Freezed) return;

        #region Movement
        if (Input.GetButton("Move_Buttons") || Input.GetButton("Move_Arrows"))
            animator.SetFloat("Speed", 1);

        if (!Input.GetButton("Move_Buttons") && !Input.GetButton("Move_Arrows"))
            animator.SetFloat("Speed", 0);
        #endregion

        #region Actions
        if (Input.GetButtonDown("Dash") && MPM.canDash && HasDash)
        {
            animator.SetTrigger("Dash");
            StartCoroutine(MPM.Slide()); 
        }

        if (Input.GetButtonDown("Slide") && MPM.canDash && MPM.IsGrounded() && HasSlide)
        {
            animator.SetTrigger("Slide");
            StartCoroutine(MPM.Slide()); 
        }

        if (Input.GetButton("Jump") && MPM.IsGrounded())
            animator.SetTrigger("Jump");

        if (MPM.IsGrounded())
        {
            animator.ResetTrigger("Jump");
            animator.SetBool("Fall", false);
        }
        else
        {
            animator.ResetTrigger("Jump");
            animator.SetBool("Fall", true);
        }
        #endregion
    
        #region Attack
        if (Input.GetMouseButtonDown(0))
        {
            if (CanAttack) Attack();
            if (CanCombo) ComboActivated = true;
        }
        #endregion
    }

    #region Attack Handler
    public void Attack()
    {
        MPM.speed = 0;
        MPM.Freezed = true;
        CanAttack = false;
        ComboActivated = false;
        CanCombo = false;
        animator.SetFloat("CurrentAttack", CurrentAttack);
        animator.SetTrigger("Attack");
    }

    public void ComboAccess() { CanCombo = true; }

    public void ComboCheck()
    {
        if (ComboActivated)
        {
            if (CurrentAttack < MaxAttack) CurrentAttack++;
            else CurrentAttack = 0;
            Attack();
        }
        else Reset();
    }
    #endregion

    public void Reset()
    {
        MPM.speed = Speed;
        MPM.Freezed = false;
        CanAttack = true;
        CanCombo = false;
        ComboActivated = false;
        CurrentAttack = 0;
    }
}
