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

    [Header("Patrol")]
    public Transform[] Patrol;
    public float Speed;
    private Transform ThisT; //This object's transform
    private int LookAt = 1; //If the LookAt is negative then the enemy goes left

    [Header("Movement")]
    private float Distance;
    private bool CanMove = true;

    private Animator animator;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ThisT = GetComponent<Transform>();
        animator.SetFloat("State", 1);
    }

    // Update is called once per frame
    void Update()
    {
        #region Distance
        if (LookAt > 0)
            Distance = (ThisT.position - Patrol[0].position).magnitude;
        else
            Distance = (ThisT.position - Patrol[1].position).magnitude;

        if (Distance < 0.1f && CanMove)
            StartCoroutine(ChangePatrolPoint());
        #endregion

        #region Movement
        if (!CanMove) return;

        if (LookAt > 0) //Enemy is moving to the right
        {
            ThisT.position = Vector2.MoveTowards(ThisT.position, Patrol[0].position, Speed);
            ThisT.localScale = Patrol[0].localScale;
        }
        else //Enemy is moving to the left
        {
            ThisT.position = Vector2.MoveTowards(ThisT.position, Patrol[1].position, Speed);
            ThisT.localScale = Patrol[1].localScale;
        }
        #endregion
    }

    IEnumerator ChangePatrolPoint()
    {
        CanMove = false;
        animator.SetFloat("State", 0);
        yield return new WaitForSeconds(3f);
        if (LookAt > 0) LookAt = -1;
        else LookAt = 1;
        animator.SetFloat("State", 1);
        CanMove = true;
    }

    #region Hit Functions
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
    #endregion
}
