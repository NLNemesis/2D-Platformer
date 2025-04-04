using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Animations;

public class AnimController : MonoBehaviour
{
    #region Variables
    [Header("Attack Variables")]
    public bool CanAttack;
    public int Attacks;
    private int CurrentAttack;
    public bool CanCombo;
    public bool ComboActivated;
    public Transform AttackPoint;
    public float AttackRange;
    public LayerMask EnemyLayer;

    [Header("References")]
    private PlayerMovement PM;
    private InputManager IM;
    private UIController UIC;
    [HideInInspector] public Animator animator;
    private AudioSource Audio;
    public AudioClip[] Clip;
    #endregion

    private void Start()
    {
        PM = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        IM = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<InputManager>();
        UIC = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<UIController>();
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PM.Health <= 0) return;

        if (PM.PlayerFreeze == false)
        {
            #region Set Movement and Jumping Animations
            if (Input.GetKey(IM.MoveLeft) || Input.GetKey(IM.MoveRight))
                animator.SetFloat("State", Mathf.Abs(PM.horizontal));
            else if (Input.GetKeyUp(IM.MoveLeft) || Input.GetKeyUp(IM.MoveRight))
                animator.SetFloat("State", 0);
            #endregion
        }

        #region Attacks
        if (Input.GetMouseButtonDown(0) && !UIC.InUI && PM.Stamina >= 30)
        {
            if (CanAttack)
            {
                CanAttack = false;
                Attack();
                Debug.Log("Check");
            }

            if (CanCombo == true)
            {
                CanCombo = false;
                ComboActivated = true;
            }
        }
        #endregion

        #region Ground Check
        if (PM.IsGrounded())
        {
            animator.ResetTrigger("Jump");
            animator.SetBool("InAir", false);
        }
        else
        {
            animator.ResetTrigger("Jump");
            animator.SetBool("InAir", true);
        }
        #endregion

        #region Death Check
        if (PM.Death == false && PM.Health <= 0)
        {
            PM.PlayerFreeze = true;
            PM.Death = true;
            animator.SetTrigger("Death");
        }
        #endregion
    }

    #region Attack Handler
    public void Attack()
    {
        PM.Stamina -= 30;
        CanAttack = false;
        CanCombo = false;
        ComboActivated = false;
        animator.SetFloat("CurrentAttack", CurrentAttack);
        animator.SetTrigger("Attack");
        if (CurrentAttack > Attacks) CurrentAttack = 0;
        else CurrentAttack++;
    }

    public void ComboAccess() { CanCombo = true; }

    public void ComboCheck()
    {
        if (ComboActivated)
            Attack();
        else
            AttackReset();
    }

    public void AttackReset()
    {
        animator.SetFloat("State", 0);
        CanAttack = true;
        CurrentAttack = 0;
        CanCombo = false;
        ComboActivated = false;
        PM.UnFreeze();
    }
    #endregion

    #region Deal Damage
    public void DealDamage(float Multiply)
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayer); //Check for the enemies
        bool[] hitEnemy = new bool[hit.Length];
        int Number = 0;
        foreach (Collider2D enemy in hit) //if we hit enemies
        {
            Debug.Log("We hit" + enemy.name);
            //enemy.GetComponent<AIMove>().TakeDamage(AttackDamage[1])
            AIMove Enemy = enemy.GetComponent<AIMove>();

            if (Enemy != null && hitEnemy[Number] == false)
            {
                hitEnemy[Number] = true;
                Enemy.TakeDamage(PM.Damage * Multiply, true);
                Number++;
                Debug.Log("I gave damage");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
    #endregion

    #region Audios
    public void PlayAudio(int Number)
    {
        Audio.clip = Clip[Number];
        Audio.Play();
    }
    #endregion

    #region Spawn Arrow
    [Header("For Arrow")]
    public GameObject ThrowablePrefab;
    public Transform ThrowableStartPoint;

    public void SpawnThrowable(int Speed)
    {
        ThrowableMovement TM = ThrowablePrefab.GetComponent<ThrowableMovement>();
        TM.Speed = Speed;
        Instantiate(ThrowablePrefab, new Vector2(ThrowableStartPoint.position.x, ThrowableStartPoint.position.y), Quaternion.identity);
    }
    #endregion
}
