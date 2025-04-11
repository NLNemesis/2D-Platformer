using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    public enum EnemyType {Dummy,Classic,Boss};
    public EnemyType Type;

    [Header("Patrol")]
    public Transform[] Point;
    public float Speed;
    private int Direction;
    private float Distance;
    private bool CanMove = true;

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
        animator.SetFloat("State", 1);
        this.transform.localScale = Point[0].localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove && Type == EnemyType.Classic)
        {
            Distance = (this.transform.position - Point[Direction].position).magnitude;
            this.transform.position = Vector2.MoveTowards(this.transform.position, Point[Direction].position, Speed);
            if (Distance == 0)
                StartCoroutine(ChangeDirection());
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
        animator.SetFloat("State", 0);
        yield return new WaitForSeconds(3f);
        if (Direction == 0)
        {
            Direction = 1;
            this.transform.localScale = Point[Direction].localScale;
        }
        else
        {
            Direction = 0;
            this.transform.localScale = Point[Direction].localScale;
        }
        animator.SetFloat("State", 1);
        CanMove = true;
    }
}
