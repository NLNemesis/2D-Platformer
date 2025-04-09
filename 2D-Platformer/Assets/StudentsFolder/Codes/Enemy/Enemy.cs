using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
    public EnemyType Type;
    public enum EnemyType {Dummy, Classic, Boss};

    [Header("Patrol")]
    public Transform[] Point;
    public float Speed;
    private bool CanMove = true;
    private int Direction = 1;
    private float Distance;

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
        if (Type == EnemyType.Classic)
        {
            #region Calculate Distance
            if (Direction == 1)
                Distance = (this.transform.position - Point[0].position).magnitude;
            else
                Distance = (this.transform.position - Point[1].position).magnitude;
            #endregion
        
            if (Distance > 0 && CanMove == true)
            {
                if (Direction == 1)
                    this.transform.position = Vector2.MoveTowards(this.transform.position, Point[0].position, Speed);
                else
                    this.transform.position = Vector2.MoveTowards(this.transform.position, Point[1].position, Speed);

                if (Distance == 0 && CanMove == true)
                    StartCoroutine(ChangeDirection());
            }
        }
    }
    #region Take Damage
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
            animator.SetTrigger("Death");
    }
    #endregion

    IEnumerator ChangeDirection()
    {
        CanMove = false;
        yield return new WaitForSeconds(3f);
        if (Direction == 1)
        {
            Direction = -1;
            this.transform.localScale = Point[1].localScale;
        }
        else
        {
            Direction = 1;
            this.transform.localScale = Point[0].localScale;
        }
        CanMove = true;
    }
}
