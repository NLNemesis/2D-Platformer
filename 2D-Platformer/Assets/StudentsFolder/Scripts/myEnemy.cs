using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myEnemy : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
    public bool freeze;
    [HideInInspector] public bool detection;
    public BoxCollider2D HitBox;

    [Header("Stats")]
    public int health;
    [HideInInspector] public bool DealDamage;

    [Header("References")]
    private Animator animator;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Take Damage
    public void TakeDamage(int value)
    {
        health -= value;
        AIFreeze();
        if (health > 0)
        {
            animator.Play("Hit");
        }
        else
        {
            health = 0;
            animator.Play("Death");
        }
    }
    #endregion

    #region Freeze/Unfreeze
    public void AIFreeze()
    {
        freeze = true;
    }

    public void AIUnfreeze()
    {
        freeze = false;
        detection = false;
        HitBox.gameObject.SetActive(true);
    }
    #endregion

    #region Deal Damage On/Off
    public void DealDamageOn()
    {
        DealDamage = true;
    }

    public void DealDamageOff()
    {
        DealDamage = false;
    }
    #endregion
}
