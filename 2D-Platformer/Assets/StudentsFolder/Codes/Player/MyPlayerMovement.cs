using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MyPlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
    public bool Freezed;
    [HideInInspector] public bool InLadder;
    [HideInInspector] public bool IsClimbing;

    [Header("Movement")]
    public float speed = 8f;
    [HideInInspector] public float originalSpeed;
    public float jumpingPower = 16f;
    private float horizontal;
    private bool isFacingRight = true;
    public float vertical;
    public float ClimbingSpeed;

    [Header("Stats")]
    public float Health;
    public float Damage;

    [Header("References")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private MyAnimController MAC;

    [Header("Unity Event")]
    public UnityEvent IsGroundedEvent;
    public UnityEvent NotIsGroundedEvent;
    #endregion

    void Awake()
    {
        originalSpeed = speed;
        MAC = gameObject.GetComponent<MyAnimController>();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

        if (Freezed) return;
        
        if (isSliding || isDashing) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

        if (!Input.GetKey(KeyCode.Space) && rb.velocity.y > 0f)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        Flip();
        IsGrounded();

        #region Box Collider Handler
        if (IsGrounded())
            IsGroundedEvent.Invoke();
        else
            NotIsGroundedEvent.Invoke();
        #endregion
    }

    private void FixedUpdate()
    {
        if (Freezed) return;

        if (!isSliding && !isDashing)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (IsClimbing == true)
        {
            rb.gravityScale = 1f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * ClimbingSpeed);
        }
        else
        {
            rb.gravityScale = 6f;
        }
    }

    #region Check if the player is grounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    #endregion

    #region Flip
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    #endregion

    #region Dash
    [Header("Dash Controller")]
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    private bool isDashing;
    [HideInInspector] public bool canDash = true;

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        Unfreeze();
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    #endregion

    #region Slide
    [Header("Slide Controller")]
    public float slidingPower = 24f;
    public float slidingTime = 0.2f;
    public float slidingCooldown = 1f;
    private bool isSliding;
    [HideInInspector] public bool canSlide = true;

    public IEnumerator Slide()
    {
        canSlide = false;
        isSliding = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * slidingPower, 0f);
        yield return new WaitForSeconds(slidingTime);
        rb.gravityScale = originalGravity;
        isSliding = false;
        Unfreeze();
        yield return new WaitForSeconds(slidingCooldown);
        canSlide = true;
    }
    #endregion

    #region Freeze & Unfreeze the player
    public void Freeze()
    {
        speed = 0;
        Freezed = true;
    }

    public void Unfreeze()
    {
        speed = originalSpeed;
        Freezed = false;
    }
    #endregion

    #region Ladder Controller
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            InLadder = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            InLadder = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            InLadder = false;
            IsClimbing = false;
        }
    }
    #endregion

    #region Take Damage
    public void TakeDamage(float Value)
    {
        if (Health > 0)
        {
            if (MAC.HitAnimation)
            {
                MAC.HitAnimation = false;
                MAC.animator.SetTrigger("Hit");
            }
            Health -= Value;

            if (Health < 0)
                MAC.animator.SetTrigger("Death");
        }
    }
    #endregion
}