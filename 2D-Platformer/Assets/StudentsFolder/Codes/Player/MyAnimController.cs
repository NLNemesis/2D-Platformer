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

    [Header("Attack Controller")]
    public int MaxAttack;
    public int CurrentAttack;
    private bool CanCombo;
    private bool ComboActivated;

    [Header("Hitbox")]
    public Transform AttackPoint;
    public float Range;
    public LayerMask EnemyLayer;

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
        #region Movement
        if (Input.GetButton("Move_Buttons") || Input.GetButton("Move_Arrows"))
            animator.SetFloat("Speed", 1);

        if (!Input.GetButton("Move_Buttons") && !Input.GetButton("Move_Arrows"))
            animator.SetFloat("Speed", 0);

        if (Input.GetButtonDown("Dash") && HasDash && MPM.canDash)
            animator.SetTrigger("Dash");

        if (Input.GetButtonDown("Slide") && HasSlide && MPM.canSlide)
            animator.SetTrigger("Slide");
        #endregion

        #region Jump
        if (Input.GetButtonDown("Jump"))
            animator.SetTrigger("Jump");

        if (MPM.IsGrounded())
            animator.SetBool("Fall", false);
        else
            animator.SetBool("Fall", true);
        #endregion

        #region Attacks
        if (Input.GetMouseButtonDown(0) && CanAttack)
            Attack();

        if (Input.GetMouseButtonDown(0) && CanCombo)
            ComboActivated = true;
        #endregion
    }

    #region Attack Handler
    public void Attack()
    {
        CanAttack = false;
        ComboActivated = false;
        animator.SetInteger("CurrentAttack", CurrentAttack);
        animator.SetTrigger("Attack");
        if (CurrentAttack == MaxAttack)
            CurrentAttack = 0;
        else
            CurrentAttack++;
    }

    public void ComboAccess() 
    { 
        CanCombo = true; 
    }

    public void ComboCheck()
    {
        if (ComboActivated)
            Attack();
        else
            Reset();
    }

    void OnDrawGizmosSelected()
    {
        if (AttackPoint != null)
            Gizmos.DrawWireSphere(AttackPoint.position, Range);
    }
    #endregion

    #region Deal Damage
    public void DealDamage(float Multiply)
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(AttackPoint.position, Range, EnemyLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            Enemy enemy = hit[i].GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(MPM.Damage * Multiply, false);
        }
    }
    #endregion

    #region Reset
    public void Reset()
    {
        CanAttack = true;
        CanCombo = false;
        ComboActivated = false;
        CurrentAttack = 0;
        MPM.Unfreeze();
    }
    #endregion
}
