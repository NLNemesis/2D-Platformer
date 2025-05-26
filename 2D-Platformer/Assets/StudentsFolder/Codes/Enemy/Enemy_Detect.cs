using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Detect : MonoBehaviour
{
    #region Variables
    private bool CanAttack;
    private Enemy enemy;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        CanAttack = enemy.CanMove;
    }

    private void OnTriggerStay2D(Collider2D Object)
    {
        if (Object.name == "Character" && CanAttack)
        {
            enemy.animator.SetTrigger("Attack");
            enemy.CanMove = false;
        }

        if (Object.name == "Character" && enemy.DealDamage)
        {
            enemy.DealDamage = false;
            enemy.CanMove = false;
            if (enemy.MAC.animator.GetBool("Parry"))
                enemy.MAC.animator.SetTrigger("ParryAttack");
            else
                enemy.MPM.TakeDamage(enemy.Power);
        }
    }

    private void OnTriggerExit2D(Collider2D Object)
    {
        if (Object.name == "Character" && enemy.DealDamage)
        {
            enemy.DealDamage = false;
            enemy.CanMove = false;
            if (enemy.MAC.animator.GetBool("Parry"))
                enemy.MAC.animator.SetTrigger("ParryAttack");
            else
                enemy.MPM.TakeDamage(enemy.Power);
        }
    }
}
