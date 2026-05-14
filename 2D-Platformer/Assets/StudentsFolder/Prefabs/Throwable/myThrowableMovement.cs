using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myThrowableMovement : MonoBehaviour
{
    #region Variables
    [Header("Controls")]
    public bool player_Throwable;
    public float Speed;
    [HideInInspector] public bool IsFacingRight;
    private Vector2 NewPlace;

    private bool Called;
    [Space(10)]
    public float DestroyTime;

    [Header("References")]
    Animator animator;
    [HideInInspector] public myPlayer player;
    [HideInInspector] public myAnimator playerAnim;
    [HideInInspector] public myEnemy enemy;
    [HideInInspector] public myEnemyDetect enemyDetect;
    #endregion

    // Update is called once per frame
    void Update()
    {
        #region Moves the Throwable
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
        #endregion
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
    public LayerMask hitLayer;

    public void Hit()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, hitLayer); //Check for the enemies
        for (int i = 0; i < hit.Length; i++)
        {
            if (player_Throwable)
            {
                myEnemy enemy = hit[i].GetComponent<myEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(playerAnim.attackPower);
                    FoundTarget();
                }
            }
            else
            {
                myPlayer Myplayer = hit[i].GetComponent<myPlayer>();
                if (Myplayer != null)
                {
                    Myplayer.LoseHP(enemyDetect.damage, true);
                    FoundTarget();
                }
                else
                    FoundTarget();
            }
        }
    }
    #endregion

    #region If it finds a target
    void FoundTarget()
    {
        Speed = 0;
        if (animator != null)
        {
            animator.SetTrigger("Hit");
            Destroy(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion

    #region Draw the hitbox gizmos
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
    #endregion
}
