using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myEnemy : MonoBehaviour
{
    public int health;
    private bool freeze;

    private Animator animator;
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
            animator.Play("Death");
        }
    }
    #endregion

    #region Freeze/Unfreeze

    #endregion
}
