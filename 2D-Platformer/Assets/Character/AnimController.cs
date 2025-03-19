using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class AnimController : MonoBehaviour
{
    #region Variables
    private bool CanTakeDamage = true;
    private bool HitEnemy;

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
        if (PM.Health > 0)
        {
            if (PM.PlayerFreeze == false && PM.State == 0)
            {
                #region Set Movement and Jumping Animations
                if (Input.GetKey(IM.MoveLeft) || Input.GetKey(IM.MoveRight))
                    animator.SetFloat("State", Mathf.Abs(PM.horizontal));
                else if (Input.GetKeyUp(IM.MoveLeft) || Input.GetKeyUp(IM.MoveRight))
                    animator.SetFloat("State", 0);
                #endregion
            }

            if (PM.IsGrounded())
                animator.SetBool("InAir", false);
            else
                animator.SetBool("InAir", true);
        }

        #region Death Check
        if (PM.Death == false && PM.Health <= 0)
        {
            PM.PlayerFreeze = true;
            PM.Death = true;
            animator.SetTrigger("Death");
        }
        #endregion
    }

    #region Attack Code
    [Header("Attack Variables")]
    public Transform AttackPoint;
    public float AttackRange;
    public LayerMask EnemyLayer;

    public void Attack()
    {
        HitEnemy = false;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayer); //Check for the enemies
        foreach (Collider2D enemy in hitEnemies) //if we hit enemies
        {
            Debug.Log("We hit" + enemy.name);
            //enemy.GetComponent<AIMove>().TakeDamage(AttackDamage[1])
            AIMove Enemy = enemy.GetComponent<AIMove>();

            if (Enemy != null && HitEnemy == false)
            {
                HitEnemy = true;
                Enemy.TakeDamage(PM.Damage, true);
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

    public void MovementReset()
    {
        PM.MovementReset();
    }
}
