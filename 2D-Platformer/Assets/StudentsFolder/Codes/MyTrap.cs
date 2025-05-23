using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTrap : MonoBehaviour
{
    #region Variables
    [Header("Damage")]
    public float DamageValue;

    private BoxCollider2D BC2D;
    private MyPlayerMovement MPM;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        BC2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player" ||  collision.name == "Character")
        {
            BC2D.enabled = false;
            MPM.TakeDamage(DamageValue);
            StartCoroutine(CollisionReset());
        }
    }

    IEnumerator CollisionReset()
    {
        yield return new WaitForSeconds(3f);
        BC2D.enabled = true;
    }
}
