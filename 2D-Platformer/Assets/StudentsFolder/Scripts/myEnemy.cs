using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myEnemy : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
    public bool freeze;
    [HideInInspector] public bool detection;
    public BoxCollider2D HitBox;

    [Header("Stats")]
    public int health;
    [HideInInspector] public bool DealDamage;

    [Header("References")]
    private Animator animator;

    public Transform leftPosition;
    public Transform rightPosition;
    private int direction = 1;
    public float Speed;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
            Move();
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
            health = 0;
            animator.Play("Death");
        }
    }
    #endregion

    #region Freeze/Unfreeze
    public void AIFreeze()
    {
        freeze = true;
    }

    public void AIUnfreeze()
    {
        freeze = false;
        detection = false;
        HitBox.gameObject.SetActive(true);
    }
    #endregion

    #region Deal Damage On/Off
    public void DealDamageOn()
    {
        DealDamage = true;
    }

    public void DealDamageOff()
    {
        DealDamage = false;
    }
    #endregion

    #region Movement
    public void Move()
    {
        float leftDistance = (leftPosition.position - this.transform.position).magnitude;
        float rightDistance = (rightPosition.position - this.transform.position).magnitude;

        if (leftDistance == 0 && direction == -1)
            direction = 1;
        else if (rightDistance == 0 && direction == 1)
            direction = -1;

        if (direction == 1)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, rightPosition.position, Speed);
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == -1)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, leftPosition.position, Speed);
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    #endregion
}
