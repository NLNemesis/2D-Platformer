using System.Collections;
using UnityEngine;

public class MyPlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    public float speed = 8f;
    public float jumpingPower = 16f;
    private float horizontal;
    private bool isFacingRight = true;

    [HideInInspector] public bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    public Rigidbody2D rb;
    public Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    #endregion

    void Start()
    {

    }

    private void Update()
    {
        if (isDashing == false)
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
                StartCoroutine(Dash());

            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing == false)
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
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    #endregion
}