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

    public void TakeDamage(float Value, bool Magic)
    {
        animator.SetTrigger("Hit");
        float Damage = 0;

        if (Magic) 
            Damage = Value - MagicResist;
        else 
            Damage = Value - Armor;

        if (Damage > 0) 
            Health -= Damage;

        if (Health <= 0) 
            this.gameObject.SetActive(false);
    }
}
