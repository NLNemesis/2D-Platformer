using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableMovement : MonoBehaviour
{
    #region Variables
    private Vector2 NewPlace;
    private bool IsFacingRight;
    public float Speed;
    public bool MagicDamage;

    private bool Called;
    [Space(10)]
    public float DestroyTime;

    PlayerMovement PM;
    Animator animator;
    #endregion

    private void Start()
    {
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        IsFacingRight = PM.isFacingRight;
        if (PM.isFacingRight)
        {
            Vector3 localScales = transform.localScale;
            localScales.x *= 1;
            transform.localScale = localScales;
        }
        else
        {
            Vector3 localScales = transform.localScale;
            localScales.x *= -1;
            transform.localScale = localScales;
        }
    }

    void Update()
    {
        //Moves the Throwable
        if (IsFacingRight)
            NewPlace = new Vector2(this.transform.position.x + 2, this.transform.position.y);
        else
            NewPlace = new Vector2(this.transform.position.x + 2 * -1, this.transform.position.y);
        this.transform.position = Vector2.MoveTowards(this.transform.position, NewPlace, Time.deltaTime * Speed);

        //Starts a Coroutine
        StartCoroutine(Duration());
        Hit();

        if (Check())
            Destroy(this.gameObject);
    }

    //Duration of the throwable object
    IEnumerator Duration()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(this.gameObject);
    }

    //Checks if the throwable hits an enemy
    public LayerMask CheckLayer;
    bool Check()
    {
        return Physics2D.OverlapCircle(this.transform.position, 0.1f, CheckLayer);
    }

    #region Attack Code
    //Damage Variables
    [Header("Attack Variables")]
    public Transform AttackPoint;
    public float AttackRange;
    public LayerMask EnemyLayer;

    public void Hit()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayer); //Check for the enemies
        for (int i = 0; i < hit.Length; i++) 
        {
            AIMove aiMove = hit[i].GetComponent<AIMove>();
            Enemy enemy = hit[i].GetComponent<Enemy>();
            if (aiMove != null)
            {
                aiMove.TakeDamage(PM.Damage, MagicDamage);
                StartCoroutine(HitEnemy());
            }
            else if (enemy != null)
            {
                enemy.TakeDamage(PM.Damage, MagicDamage);
                StartCoroutine(HitEnemy());
            }
        }
    }

    //Destroy the throwable object
    IEnumerator HitEnemy()
    {
        Speed = 0;
        if (animator != null)
        {
            animator.SetTrigger("Hit");
            yield return new WaitForSeconds(0.3f);
            Destroy(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    //Draw the hitbox gizmos
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
    #endregion
}
