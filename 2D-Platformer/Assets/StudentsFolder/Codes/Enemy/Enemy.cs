using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    public float Health;
    public float Armor;
    public float MR; //MagicReist
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float Value)
    {
        float NewHealth = Health - (Value + Armor);
        if (NewHealth > 0)
            Health = NewHealth;
        else
            this.gameObject.SetActive(false);
    }
}
