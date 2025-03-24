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

    [Header("Attacks")]
    public int MaxAttack;
    public bool CanCombo;
    public bool Combo;
    public int CurrentAttack;

    [Header("References")]
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
            StartCoroutine(MPM.Dash()); 
        }

        if (Input.GetButtonDown("Slide") && MPM.canDash && MPM.IsGrounded() && HasSlide)
        {
            animator.SetTrigger("Slide");
            StartCoroutine(MPM.Dash()); 
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

        #region Attacks
        if (Input.GetMouseButtonDown(0) && CanAttack)
            Attack();

        if (Input.GetMouseButtonDown(0) && CanCombo)
            Combo = true;
        #endregion
    }

    #region Attacks Handler
    public void Attack()
    {
        StopCoroutine(AttackReset());
        Combo = false;
        CanAttack = false;
        animator.SetFloat("Attack", CurrentAttack);
        animator.SetTrigger("Attack Action");
    }

    public void ComboActivated() { CanCombo = true; }

    public void ForceAttack()
    {
        if (Combo)
        {
            if (CurrentAttack <= MaxAttack)
            {
                CurrentAttack++;
                Attack();
            }
            else
            {
                CurrentAttack = 0;
                Attack();
            }
        }
    }

    IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(2f);
        CanCombo = false;
        CanAttack = true;
        CurrentAttack = 0;
    }
    #endregion

    public void Reset()
    {
        MPM.Freezed = false;
        CanCombo = false;
        CanAttack = true;
        CurrentAttack = 0;
    }
}
