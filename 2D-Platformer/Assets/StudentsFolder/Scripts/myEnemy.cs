using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class myEnemy : MonoBehaviour
{
    #region Variables
    public string category; //classic-follow-boss

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

    [Header("Events")]
    public UnityEvent deathEvent;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (category == "Boss")
            freeze = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze && health > 0 && category == "Classic")
            Move();

        if (!freeze && health > 0 && category == "Boss")
            Move_Boss();
    }

    #region Take Damage
    public void TakeDamage(int value)
    {
        if (category != "Dummy")
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
            deathEvent.Invoke();
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
            animator.SetFloat("Movement", 1);
            this.transform.localScale = rightPosition.localScale;
        }
        else if (direction == -1)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, leftPosition.position, Speed);
            animator.SetFloat("Movement", 1);
            this.transform.localScale = leftPosition.localScale;
        }
    }
    #endregion

    #region Boss Movement
    public Transform player;
    public Vector3 rightScale;
    public Vector3 leftScale;
    public void Move_Boss()
    {
        float distance = player.transform.position.x - this.transform.position.x;

        if (distance > 0)
            this.transform.localScale = rightScale;
        else
            this.transform.localScale = leftScale;

        Vector2 newPlace = new Vector2(player.transform.position.x, 0);
        this.transform.position = Vector2.MoveTowards(this.transform.position, newPlace, Speed);
        animator.SetFloat("Movement", 1);
    }
    #endregion
}
