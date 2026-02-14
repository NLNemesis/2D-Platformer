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
    private myGameManager myGM;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<myPlayer>();
        animator = GetComponent<Animator>();
        myGM = FindObjectOfType<myGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myGM.UI != 0) return;

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
