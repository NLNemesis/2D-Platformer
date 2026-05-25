using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myAnimator : MonoBehaviour
{
    public int attackPower;
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
        if (!player.IsGrounded()) return;

        if (!player.frozen)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
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
        bool[] dealt;
        hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        dealt = new bool[hit.Length];
        for (int i = 0; i < hit.Length; i++)
        {
            if (!dealt[i])
            {
                dealt[i] = true;

                myEnemy enemy = hit[i].GetComponent<myEnemy>();
                if (enemy != null)
                    enemy.TakeDamage(attackPower, true);
                else
                {
                    enemy = hit[i].GetComponentInParent<myEnemy>();
                    enemy.TakeDamage(attackPower, true);
                }
            }
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
