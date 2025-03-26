using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIMove : MonoBehaviour
{
    #region Variables
    public enum EnemyType{Classic, MiniBoss, Boss}
    public EnemyType enemyType;

    [Header("Movement")]
    public float Speed;
    public Transform[] PatrolPlaces;
    public float WaitForPatrol;
    private bool CanMove = true;
    private float Distance;
    private int CurrentPatrol;

    [Header("Stats")]
    public bool AIFreeze;
    public float Health;
    public bool PhysicalDamage;
    public float Damage;
    public float Armor;
    public float MagicResist;
    public float XP;
    [HideInInspector] public float CurrentSpeed;
    [HideInInspector] public bool Dead;

    [Header("References")]
    public UnityEvent DeathEvent;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public PlayerMovement PM;
    [HideInInspector] public Animator animator;
    private AIDetect aiDetect;
    private Vector2 NewTarget;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("/MaxPrefab/Player");
        PM = Player.GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        if (AIFreeze != true) animator.SetFloat("Movement", 1);
        aiDetect = GetComponentInChildren<AIDetect>();
        CurrentSpeed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (AIFreeze == false)
        {
            #region Movement
            if (enemyType == EnemyType.Classic)
            {
                #region Classic Enemy Movement
                if (CanMove == true)
                {
                    Distance = (this.transform.position - PatrolPlaces[CurrentPatrol].position).magnitude;
                    NewTarget = new Vector2(PatrolPlaces[CurrentPatrol].position.x, this.transform.position.y);
                    this.transform.position = Vector2.MoveTowards(this.transform.position, NewTarget, Speed);
                    this.transform.localScale = PatrolPlaces[CurrentPatrol].localScale;

                    if (Distance <= 1f)
                    {
                        StartCoroutine(NewPatrol());
                    }
                }
                #endregion
            }
            else if (enemyType != EnemyType.Classic)
            {
                #region Boss Enemy Movement
                animator.SetFloat("Movement", 1);
                Distance = (this.transform.position - PatrolPlaces[CurrentPatrol].position).magnitude;
                NewTarget = new Vector2(PatrolPlaces[CurrentPatrol].position.x, this.transform.position.y);
                this.transform.position = Vector2.MoveTowards(this.transform.position, NewTarget, Speed);

                //Look at the left
                if (this.transform.position.x > NewTarget.x)
                {
                    this.transform.localScale = new Vector3(1, 1, 1);
                }

                //Look at the right
                if (this.transform.position.x < NewTarget.x)
                {
                    this.transform.localScale = new Vector3(-1, 1, 1);
                }
                #endregion
            }
            #endregion
        }
    }

    #region New Patrol
    IEnumerator NewPatrol()
    {
        CanMove = false;
        animator.SetFloat("Movement", 0);
        CurrentPatrol += 1;
        if (CurrentPatrol > PatrolPlaces.Length - 1)
        {
            CurrentPatrol = 0;
        }
        yield return new WaitForSeconds(WaitForPatrol);
        CanMove = true;
        animator.SetFloat("Movement", 1);
    }
    #endregion

    #region Dealing Damage On Off and Movement Reset
    public void DealingDamageOn()
    {
        aiDetect.HitBox.enabled = true;
        aiDetect.DealingDamage = true;
    }

    public void DealingDamageOff()
    {
        aiDetect.HitBox.enabled = false;
        aiDetect.DealingDamage = false;
    }

    public void MovementReset()
    {
        aiDetect.HitBox.enabled = true;
        AIFreeze = false;
        animator.ResetTrigger("Attack");
        Speed = CurrentSpeed;
    }
    #endregion

    #region Take Damage
    public void TakeDamage(float Value, bool PhysicalDamage)
    {
        if (Health > 0 && Dead == false)
        {
            if (PhysicalDamage == false)
            {
                float NewValue = Value - Armor;
                if (NewValue > 0)
                {
                    Health -= NewValue;
                }
            }
            else
            {
                float NewValue = Value - MagicResist;
                if (NewValue > 0)
                {
                    Health -= NewValue;
                }
            }

            if (Health <= 0)
            {
                Death();
            }
            else
            {
                animator.SetTrigger("Hit");
            }
        }
    }
    #endregion

    public void Death()
    {
        StartCoroutine(PM.GainXP(XP));
        Dead = true;
        AIFreeze = true;
        animator.SetTrigger("Dead");
        BoxCollider2D BC = GetComponent<BoxCollider2D>();
        BC.enabled = false;
        DeathEvent.Invoke();
    }
}
