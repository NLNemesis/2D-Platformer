using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class myPlayer : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
    public bool frozen;
    [HideInInspector] public bool inLadder;
    [HideInInspector] public bool isClimbing;
    [HideInInspector] public bool inAir;

    [Header("Movement")]
    public float speed = 8f;
    public float ClimbingSpeed;
    [HideInInspector] public float originalSpeed;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    private float horizontal;
    private float vertical;

    [Header("References")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Animator animator;

    [Header("Unity Event")]
    public UnityEvent IsGroundedEvent;
    public UnityEvent NotIsGroundedEvent;
    #endregion

    void Awake()
    {
        originalSpeed = speed;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
        if (frozen) return;
        if (isSliding || isDashing) return;

        HandlePlayerInput();
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
        if (frozen) return;

        if (!isSliding && !isDashing)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (isClimbing == true)
        {
            rb.gravityScale = 1f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * ClimbingSpeed);
        }
        else
        {
            rb.gravityScale = 6f;
        }
    }

    #region Handle Player Input
    void HandlePlayerInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0 && IsGrounded() && !Input.GetKey(KeyCode.Space))
            animator.SetBool("Walk", true);
        else if (horizontal == 0 && IsGrounded() && !Input.GetKey(KeyCode.Space))
            animator.SetBool("Walk", false);


        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");
        }
        if (!Input.GetKey(KeyCode.Space) && rb.velocity.y > 0f)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }
    #endregion

    #region Check if the player is grounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Grounded()
    {
        Debug.Log("Grounded");
    }
    
    public void NotGrounded()
    {
        Debug.Log("Not Grounded");
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
        frozen = true;
    }

    public void Unfreeze()
    {
        speed = originalSpeed;
        frozen = false;
    }
    #endregion

    #region Ladder Controller
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            inLadder = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            inLadder = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            inLadder = false;
            isClimbing = false;
        }
    }
    #endregion

    #region HP
    public GameObject[] Hearts;
    private int HP = 7;

    public void GainHP(int hp)
    {
        HP += hp;
        if (HP > 7)
            HP = 7;

        for (int i = 0; i < Hearts.Length; i++)
            Hearts[i].SetActive(false);

        for (int i = 0; i < HP; i++)
            Hearts[i].SetActive(true);
    }

    public void LoseHP(int hp)
    {
        frozen = true;
        HP -= hp;

        if (HP > 0)
        {
            for (int i = 0; i < Hearts.Length; i++)
                Hearts[i].SetActive(false);

            for (int i = 0; i < HP; i++)
                Hearts[i].SetActive(true);

            animator.Play("Hit");
        }
        else
        {
            for (int i = 0; i < Hearts.Length; i++)
                Hearts[i].SetActive(false);
            animator.Play("Death");
        }
    }
    #endregion
}