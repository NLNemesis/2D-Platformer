using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    #region Variables
    public enum EnemyType{Dummy,Classic,Boss};
    public EnemyType Type;

    [Header("Controller")]
    public Transform[] Point;
    public float Speed;
    [HideInInspector] public bool CanMove = true;
    private int Direction;
    private float Distance;

    [Header("Stats")]
    public float Health;
    public float Armor;
    public float MagicResist;
    public float Power;
    [HideInInspector] public bool DealDamage;


    [Header("References")]
    [HideInInspector] public MyPlayerMovement MPM;
    [HideInInspector] public Animator animator;
    #endregion
        
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("State", 1);
        this.transform.localScale = Point[0].localScale;
        MPM = GameObject.Find("/Character Prefab/Character").GetComponent<MyPlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove && Type == EnemyType.Classic)
        {
            #region Distance
            Distance = (this.transform.position - Point[Direction].position).magnitude;
            #endregion

            #region Movement
            this.transform.position = Vector2.MoveTowards(this.transform.position,Point[Direction].position, Speed);
            #endregion

            if (Distance == 0)
                StartCoroutine(ChangeDirection());
        }
    }

    #region Take Damage
    public void TakeDamage(float Value, bool Magic)
    {
        if (Health > 0)
        {
            animator.SetTrigger("Hit");

            float Damage = 0;
            if (Magic) Damage = Value - MagicResist;
            else Damage = Value - Armor;

            if (Damage > 0) Health -= Damage;

            if (Health <= 0)
            {
                animator.SetBool("Death", true);
                CanMove = false;
            }
        }
    }
    #endregion

    #region Change Direction
    IEnumerator ChangeDirection()
    {
        CanMove = false;
        animator.SetFloat("State", 0);
        yield return new WaitForSeconds(3f);
        if (Direction == 0)
        {
            Direction = 1;
            this.transform.localScale = Point[1].localScale;
        }
        else
        {
            Direction = 0;
            this.transform.localScale = Point[0].localScale;
        }
        animator.SetFloat("State", 1);
        CanMove = true;
    }
    #endregion

    #region Attack Functions
    public void StartDealingDamage() { DealDamage = true; }
    public void StopDealingDamage() { DealDamage = false; }
    public void AttackReset()
    {
        DealDamage = false;
        CanMove = true;
    }
    #endregion
}
