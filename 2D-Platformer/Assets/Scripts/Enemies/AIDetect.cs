using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetect : MonoBehaviour
{
    #region Variables
    [Header("For Detect")]
    [HideInInspector] public bool DealingDamage;

    [Header("References")]
    [HideInInspector] public BoxCollider2D HitBox;
    private AIMove aiMove;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        aiMove = GetComponentInParent<AIMove>();
        HitBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aiMove.Health <= 0)
        {
            aiMove.AIFreeze = true;
        }
    }

    #region On Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        { 
            if (aiMove.PM.Health > 0 && aiMove.AIFreeze == false)
            {
                HitBox.enabled = false;
                aiMove.AIFreeze = true;
                aiMove.animator.SetTrigger("Attack");
                aiMove.CurrentSpeed = aiMove.Speed;
                aiMove.Speed = 0;
            }

            if (aiMove.PM.Health > 0 && DealingDamage == true)
            {
                DealingDamage = false;
                aiMove.PM.TakeDamage(aiMove.Damage, aiMove.PhysicalDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (aiMove.PM.Health > 0 && aiMove.AIFreeze == false)
            {
                HitBox.enabled = false;
                aiMove.AIFreeze = true;
                aiMove.animator.SetTrigger("Attack");
            }

            if (aiMove.PM.Health > 0 && DealingDamage == true)
            {
                DealingDamage = false;
                aiMove.PM.TakeDamage(aiMove.Damage, aiMove.PhysicalDamage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (aiMove.PM.Health > 0 && DealingDamage == true)
            {
                DealingDamage = false;
                aiMove.PM.TakeDamage(aiMove.Damage, aiMove.PhysicalDamage);
            }
        }
    }
    #endregion

    #region Dealing Damage On Off and Movement Reset
    public void DealingDamageOn()
    {
        HitBox.enabled = false;
        HitBox.enabled = true;
        DealingDamage = true;
    }

    public void DealingDamageOff()
    {
        HitBox.enabled = false;
        DealingDamage = false;
    }

    public void MovementReset()
    {
        HitBox.enabled = true;
        aiMove.AIFreeze = false;
        aiMove.animator.ResetTrigger("Attack");
    }
    #endregion
}
