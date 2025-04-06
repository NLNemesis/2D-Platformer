using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    public float Health;
    public float Armor;
    public float MagicResist;

    public Animator animator;
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float Value, bool Magic)
    {
        animator.SetTrigger("Hit");
        float Damage = 0;

        if (Magic == true)
            Damage = Value - MagicResist;
        else
            Damage = Value - Armor;

        if (Damage > 0)
            Health -= Damage;

        if (Health <= 0)
            this.gameObject.SetActive(false);
    }
}
