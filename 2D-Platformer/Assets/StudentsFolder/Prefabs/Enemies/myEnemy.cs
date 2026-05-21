using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class myEnemy : MonoBehaviour
{
    #region Variables
    [Tooltip("Classic or Boss")]
    public string category; //classic-follow-boss
    [Header("Controller")]
    public bool freeze;
    public bool guard;
    [HideInInspector] public bool detection;
    public BoxCollider2D HitBox;
    [Header("Stats")]
    public int health;
    [HideInInspector] public bool DealDamage;
    [Header("References")]
    private Animator animator;

    // OLD: public Transform leftPosition;
    // OLD: public Transform rightPosition;
    // OLD: private int direction = 1;

    [Header("Classic Waypoints")]
    public Transform[] waypoints;       // Assign in Inspector
    private int currentWaypointIndex = 0;

    public float Speed;
    [Header("Events")]
    public UnityEvent deathEvent;
    #endregion
    void Awake()
    {
        animator = GetComponent<Animator>();
        if (category == "Boss")
            freeze = true;
    }

    void Update()
    {
        if (guard) return;

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
        // Safety check
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];

        // Face the direction of movement
        float distance = target.position.x - this.transform.position.x;
        if (distance > 0)
            this.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (distance < 0)
            this.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // Move towards current waypoint
        this.transform.position = Vector2.MoveTowards(this.transform.position, target.position, Speed);
        animator.SetFloat("Movement", 1);

        // Check if we reached the waypoint, then advance to next
        if (Vector2.Distance(this.transform.position, target.position) < 0.05f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
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

    #region Spawn Throwable
    [Header("Throwable")]
    public Transform spawnPoint;
    public GameObject throwable;
    public float speed;

    public void SpawnThrowable()
    { 
        GameObject obj = Instantiate(throwable, spawnPoint.position, spawnPoint.rotation);
        myThrowableMovement myTM = obj.GetComponent<myThrowableMovement>();
        myTM.enemy = this;
        myTM.enemyDetect = this.GetComponentInChildren<myEnemyDetect>();
        myTM.player = FindObjectOfType<myPlayer>();
        myTM.playerAnim = FindObjectOfType<myAnimator>();
        myTM.Speed = speed;

        if (this.transform.localScale.x > 0)
        {
            myTM.IsFacingRight = true;
            myTM.transform.localScale *= 1;
        }
        else
        {
            myTM.IsFacingRight = false;
            myTM.transform.localScale *= -1;
        }
    }
    #endregion
}