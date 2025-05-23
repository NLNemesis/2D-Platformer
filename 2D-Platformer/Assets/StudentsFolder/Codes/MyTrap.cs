using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTrap : MonoBehaviour
{
    #region Variables
    [Header("Damage")]
    public bool MagicDamage;
    public float DamageValue;

    private BoxCollider2D BC2D;
    private MyPlayerMovement MPM;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        BC2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
