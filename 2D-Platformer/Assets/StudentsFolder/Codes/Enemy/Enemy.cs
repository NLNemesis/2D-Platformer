using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    public float Health;
    public float Armor;
    public float MR;
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
        float Damage = 0;
        if (Magic == true)
            Damage = Value - MR;
        else
            Damage = Value - Armor;

        if (Damage > 0)
        {
            if (Health - Damage > 0)
                Health -= Damage;
            else
                this.gameObject.SetActive(false);
        }
    }
}
