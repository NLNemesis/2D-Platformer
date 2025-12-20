using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myAnimator : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;

    private Animator animator;
    private myPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<myPlayer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.frozen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.Play("Attack");
                player.Freeze();
            }
        }
    }

    #region Attack Handler
    public void DealDamage()
    {
        Collider2D[] hit;
        hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            myEnemy enemy = hit[i].GetComponent<myEnemy>();
            enemy.TakeDamage(1);
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
