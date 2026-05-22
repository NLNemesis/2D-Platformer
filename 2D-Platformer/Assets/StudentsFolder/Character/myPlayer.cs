using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class myPlayer : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
    public bool frozen;
    public bool inLadder;
    public bool isClimbing;
    public bool inAir;

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
    public float groundRange;
    public Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Animator animator;
    private Animator canvasAnimator;
    public myGameManager gm;

    [Header("Unity Event")]
    public UnityEvent IsGroundedEvent;
    public UnityEvent NotIsGroundedEvent;
    #endregion

    void Awake()
    {
        originalSpeed = speed;
        animator = GetComponent<Animator>();
        Canvas canvas = this.transform.root.GetComponentInChildren<Canvas>();
        canvasAnimator = canvas.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!frozen && HP > 0)
            HandlePlayerInput();

        if (Time.timeScale == 0) return;
        if (frozen) return;
        if (isSliding || isDashing) return;
        if (HP < 1) return;

        #region Climbing Controller
        if (!isSliding && !isDashing)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (isClimbing == true)
        {
            rb.gravityScale = 3f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * ClimbingSpeed);
        }
        else
        {
            rb.gravityScale = 6f;
        }
        #endregion

        if (isClimbing) return;

        Flip();
        IsGrounded();

        #region Box Collider Handler
        if (IsGrounded())
        {
            IsGroundedEvent.Invoke();
            animator.SetBool("Falling", false);
        }
        else
        {
            NotIsGroundedEvent.Invoke();
            animator.SetBool("Falling", true);
        }
        #endregion
    }

    #region Handle Player Input
    void HandlePlayerInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // Keyboard + Left Stick X
        vertical = Input.GetAxisRaw("Vertical");     // Keyboard + Left Stick Y

        // --- Climbing ---
        if (inLadder && vertical != 0)
            isClimbing = true;

        if (inLadder && vertical > 0 && IsGrounded())
        {
            isClimbing = true;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        if (!inLadder)
            isClimbing = false;

        // --- Unified jump input (Space OR controller Jump button) ---
        bool jumpPressed = Input.GetKey(KeyCode.Space) || Input.GetButton("Jump");
        bool jumpDown = Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump");

        // --- Walk animation ---
        if (!isClimbing)
        {
            if (horizontal != 0 && IsGrounded() && !jumpPressed)
                animator.SetBool("Walk", true);
            else if (horizontal == 0 && IsGrounded() && !jumpPressed)
                animator.SetBool("Walk", false);
        }
        else
        {
            if ((vertical != 0 || horizontal != 0) && !jumpPressed)
            {
                animator.SetBool("Climb", true);
                animator.SetBool("Climbing", true);
            }
            else if (vertical == 0 && horizontal == 0 && !jumpPressed)
                animator.SetBool("Climbing", false);
        }

        if (Time.timeScale == 0) return;
        if (frozen) return;
        if (isSliding || isDashing) return;

        // --- Jump (disabled while climbing) ---
        if (jumpDown && IsGrounded() && !isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");
        }
        if (!jumpPressed && rb.velocity.y > 0f && !isClimbing)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }
    #endregion

    #region Check if the player is grounded
    public bool IsGrounded()
    {
        if (isClimbing) return false;
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(groundRange, groundRange), 0f, groundLayer);
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
        {
            inLadder = true;
        }
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
            animator.SetBool("Climb", false);
            animator.SetBool("Climbing", false);
        }
    }
    #endregion

    #region HP
    public GameObject[] Hearts;
    [HideInInspector] public int HP = 7;

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

    public void LoseHP(int hp, bool hitAnim)
    {
        frozen = true;
        HP -= hp;

        if (HP > 0)
        {
            for (int i = 0; i < Hearts.Length; i++)
                Hearts[i].SetActive(false);

            for (int i = 0; i < HP; i++)
                Hearts[i].SetActive(true);

            if (hitAnim)
                animator.Play("Hit");
        }
        else
        {
            for (int i = 0; i < Hearts.Length; i++)
                Hearts[i].SetActive(false);
            animator.Play("Death");
            canvasAnimator.Play("Death_Screen");
            gm.Toggle_Cursor(true);
            Freeze();
        }
    }
    #endregion

    #region Load HP
    public void LoadHP(int hp)
    {
        HP = hp;
        for (int i = 0; i < Hearts.Length; ++i)
            Hearts[i].SetActive(false);

        for (int i = 0; i < hp; ++i)
            Hearts[i].SetActive(true);
    }
    #endregion

    #region Draw Gizmos
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundRange, groundRange, 0f));
    }
    #endregion
}