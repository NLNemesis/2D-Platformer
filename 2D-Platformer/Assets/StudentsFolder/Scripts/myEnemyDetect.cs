using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myEnemyDetect : MonoBehaviour
{
    #region Variables

    public int damage;
    public myPlayer player;
    public myEnemy enemy;
    public Animator animator;
    #endregion

    private void OnTriggerStay2D(Collider2D Object)
    {
        if (Object.name == "Player" && !enemy.freeze)
        {
            enemy.freeze = true;
            animator.Play("Attack");
        }

        if (enemy.freeze && enemy.DealDamage)
        {
            player.LoseHP(damage);
            enemy.DealDamage = false;
            this.gameObject.SetActive(false);
        }
    }
}
