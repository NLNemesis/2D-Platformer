using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MyPlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
    public bool Freezed;

    [Header("Movement")]
    public float speed = 8f;
    public float jumpingPower = 16f;
    private float horizontal;
    private bool isFacingRight = true;

    [Header("Slide Variables")]
    public float slidingPower = 24f;
    public float slidingTime = 0.2f;
    public float slidingCooldown = 1f;
    private bool isSliding;
    [HideInInspector] public bool canDash = true;

    [Header("References")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Unity Event")]
    public UnityEvent IsGroundedEvent;
    public UnityEvent NotIsGroundedEvent;
    #endregion

    void Start()
    {

    }

    private void Update()
    {
        if (Freezed) return;
        
        if (isSliding) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
                StartCoroutine(Dash());

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
        if (isSliding == false)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
    public IEnumerator Dash()
    {
        canDash = false;
        isSliding = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * slidingPower, 0f);
        yield return new WaitForSeconds(slidingTime);
        rb.gravityScale = originalGravity;
        isSliding = false;
        yield return new WaitForSeconds(slidingCooldown);
        canDash = true;
    }
    #endregion

    #region Freeze & Unfreeze the player
    public void Freeze()
    {
        Freezed = true;
    }

    public void Unfreeze()
    {
        Freezed = false;
    }
    #endregion
}