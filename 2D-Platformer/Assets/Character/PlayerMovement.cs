using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Threading;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    public Sprite PlayerIcon;
    [HideInInspector] public int State; //0 = Free, 1 = Locked
    [HideInInspector] public bool InInteaction;
    [HideInInspector] public float horizontal = 0f;
    private float vertical = 0f;
    private bool CanIncrease;
    [HideInInspector] public bool InLadder;
    private bool IsClimbing;
    public bool InAction;

    [Header("Player")]
    public float Speed;
    public float JumpingPower;
    public float ClimbingSpeed;
    [HideInInspector] public bool isFacingRight = true;
    private float OriginalSpeed;
    public int Gold;
    [HideInInspector] public bool Freezed;
    [HideInInspector] public bool CanJump;
    [HideInInspector] public bool Jump;
    [HideInInspector] public bool Crouch;
    [HideInInspector] public bool Invisible;
    [HideInInspector] public bool Death;

    [Header("Health")]
    public float Health;
    public float MaxHealth;

    [Header("Mana")]
    public float Mana;
    public float MaxMana;

    [Header("Stamina")]
    public float Stamina;
    public float MaxStamina;
    public float StaminaRegent;
    public float IncreaseDuration;

    [Header("Dash")]
    public bool CanDash;
    public float DashStaminaRequirment;
    private bool IsDashing;
    public float DashPower = 24;
    public float DashTimer = 0.2f;
    public float DashCooldown = 1f;

    [Header("Slide")]
    public bool canSlide;
    public float SlideStaminaRequirment;
    private bool IsSliding;
    public float SlidePower = 24;
    public float SlideTimer = 0.2f;
    public float SlideCooldown = 1f;

    [Header("Stats")]
    public float Damage;
    public float SkillDamage;
    public float Armor;
    public float MagicResist;

    [Header("Gathering Tier")]
    public int AxeTier;
    public int PickaxeTier;
    public int KnifeTier;

    [Header("References")]
    [HideInInspector] public Rigidbody2D rb;
    private InputManager IM;
    private UIController UIC;
    [HideInInspector] public AnimController AC;
    public Transform GroundCheck;
    public LayerMask GroundLayer;

    [Header("UI")]
    public Slider[] Slider; //0 HP Slider, 1 Mana Slider, 2 Stamina Slider
    public TextMeshProUGUI[] UIText;

    [Header("Experience Points")]
    public float[] XPScale;
    public Slider XPSlider;
    public TextMeshProUGUI LevelText;
    [HideInInspector] public int Level;
    [HideInInspector] public float CurrentXP;

    [Header("Events")]
    public UnityEvent[] Event; //0 Jumping, 1 Landing

    [Header("Self Talk")]
    public GameObject SelfTalkObject;
    public TypeWritingEffect TWE;
    #endregion

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        IM = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<InputManager>();
        UIC = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<UIController>();
        rb = GetComponent<Rigidbody2D>();
        AC = GetComponent<AnimController>();
        OriginalSpeed = Speed;
        #region Assign XP Scale 
        for (int i = 0; i < XPScale.Length; i++)
        {
            XPScale[i] = 250 * i;
        }
        XPSlider.maxValue = XPScale[0];
        #endregion
    }

    private void Update()
    {
        if (Health > 0 && State == 0)
        {
            #region Player Inputs
            if (!Freezed)
            {
                #region Speed controller with InAction variable
                if (InAction)
                    Speed = 0;
                else
                    Speed = OriginalSpeed;
                #endregion

                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");

                if (!InAction)
                {
                    #region Jumping
                    if (Input.GetKeyDown(IM.Jump) && Stamina >= 15 && IsGrounded())
                    {
                        //InAction = true;
                        AC.animator.SetTrigger("Jump");
                        AC.animator.SetBool("InAir", true);
                        Jump = true;
                        Stamina -= 15f;
                        rb.velocity = new Vector2(rb.velocity.x, JumpingPower);
                    }

                    if (Input.GetKeyUp(IM.Jump) && rb.velocity.y > 0f)
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                    #endregion

                    #region Dash
                    if (Input.GetKeyDown(IM.Dash) && Stamina >= 15 && CanDash == true)
                        StartCoroutine(Dash(1));
                    #endregion

                    #region Slide
                    if (Input.GetKeyDown(IM.Slide) && Stamina >= 15 && canSlide == true)
                    {
                        Stamina -= SlideStaminaRequirment;
                        StartCoroutine(Slide());
                    }
                    #endregion
                }

                #region Ladder
                if (InLadder && Mathf.Abs(vertical) > 0f)
                {
                    IsClimbing = true;
                }
                #endregion

                Flip();
            }
            #endregion

            #region Stamina System
            if (InAction == false)
                if (Stamina < MaxStamina && CanIncrease == true)
                    StartCoroutine(StaminaIncrease());
                else
                    StopCoroutine(StaminaIncrease());
            else
                StopCoroutine(StaminaIncrease());

            if (Stamina > MaxStamina)
                Stamina = MaxStamina;
            #endregion
        }

        #region Assing UI
        Slider[0].maxValue = MaxHealth;
        Slider[0].value = Health;
        Slider[1].maxValue = MaxMana;
        Slider[1].value = Mana;
        Slider[2].maxValue = MaxStamina;
        Slider[2].value = Stamina;
        XPSlider.value = CurrentXP;
        LevelText.text = Level.ToString();
        UIText[3].text = Gold.ToString();
        #endregion

        #region Ground Handler
        if (IsGrounded())
            Event[1].Invoke();
        else
            Event[0].Invoke();
        #endregion
    }

    #region Move The Player
    private void FixedUpdate()
    {
        //Move the player
        if (Freezed == false)
            rb.velocity = new Vector2(horizontal * Speed, rb.velocity.y);

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
    #endregion

    #region Ground Check
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }
    #endregion

    #region Flip
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScales = transform.localScale;
            localScales.x *= -1;
            transform.localScale = localScales;
        }
    }
    #endregion

    #region Dash
    public IEnumerator Dash(int Animation)
    {
        Freezed = true;
        CanIncrease = false;
        InAction = true;
        if (Animation > 0)
        {
            StopCoroutine(StaminaIncrease());
            Stamina -= DashStaminaRequirment;
            AC.animator.SetTrigger("Dash");
        }
        CanDash = false;
        IsDashing = true;
        float OriginalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * DashPower, 0f);
        yield return new WaitForSeconds(DashTimer);
        rb.gravityScale = OriginalGravity;
        IsDashing = false;
        MovementReset();
        yield return new WaitForSeconds(DashCooldown);
        CanDash = true;
    }
    #endregion

    #region Slide
    IEnumerator Slide()
    {
        Freezed = true;
        CanIncrease = false;
        InAction = true;
        AC.animator.SetTrigger("Slide");
        canSlide = false;
        IsSliding = true;
        float OriginalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * SlidePower, 0f);
        yield return new WaitForSeconds(SlideTimer);
        rb.gravityScale = OriginalGravity;
        IsSliding = false;
        MovementReset();
        yield return new WaitForSeconds(SlideCooldown);
        canSlide = true;
    }
    #endregion

    #region Resets
    public void Freeze()
    {
        Freezed = true;
        InAction = true;
        AC.CanAttack = false;
    }

    public void UnFreeze()
    {
        Freezed = false;
        InAction = false;
        AC.CanAttack = true;
        StartCoroutine(StaminaIncrease());
    }

    public void MovementReset()
    {
        InAction = false;
        Freezed = false;
        State = 0;
        StartCoroutine(StaminaIncrease());
    }

    //I use the player state to block the players movement
    //When he opens a shop or interacts with the environment
    public void PlayerState()
    {
        if (State == 0)
        {
            State = 1;
            Speed = 0;
        }
        else
        {
            State = 0;
            Speed = OriginalSpeed;
        }
    }
    #endregion

    #region Stamina Lerp
    public IEnumerator StaminaIncrease()
    {
        CanIncrease = false;
        float Timer = 0f;
        float StaminaEndValue = Stamina + StaminaRegent;
        while (Timer < IncreaseDuration)
        {
            if(InAction == false)
            {
                Timer += Time.deltaTime;
                float Step = Timer / IncreaseDuration;

                Stamina = Mathf.Lerp(Stamina, StaminaEndValue, Step);

                yield return null;
            }
            else
            {
                Timer = IncreaseDuration;
            }
        }
        CanIncrease = true;
    }
    #endregion

    #region Heal And ManaRegend
    public IEnumerator StatsRegend(int Amount, bool Heal)
    {
        float Duration = 0.3f;
        float Timer = 0;
        float NewHealth = Health + Amount;
        float NewMana = Mana + Amount;
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            float Step = Timer / Duration;

            if (Heal == true)
            {
                Health = Mathf.Lerp(Health, NewHealth, Step);
                if (Health > MaxHealth)
                {
                    Health = MaxHealth;
                    Timer = Duration;
                }
            }
            else
            {
                Mana = Mathf.Lerp(Mana, NewMana, Step);
                if (Mana > MaxMana)
                {
                    Mana = MaxMana;
                    Timer = Duration;
                }
            }

            yield return null;
        }
    }
    #endregion

    #region Take Damage
    public void TakeDamage(float Value, bool PhysicalDamage)
    {
        Freeze();
        AC.animator.SetTrigger("Hit");

        if (PhysicalDamage == false)
        {
            float NewValue = Value - Armor;
            if (NewValue > 0)
            {
                Health -= NewValue;
            }
        }
        else
        {
            float NewValue = Value - MagicResist;
            if (NewValue > 0)
            {
                Health -= NewValue;
            }
        }

        if (Health <= 0)
        {
            State = 1;
            Death = true;
            Freezed = true;
            AC.animator.SetTrigger("Death");
            Speed = 0;
        }
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

    #region Jump Check
    public void JumpCheckFunction()
    {
        Jump = false;
        AC.animator.SetBool("InAir", false);
        MovementReset();
    }
    #endregion

    #region XP System
    public IEnumerator GainXP(float Value)
    {
        if (Level < XPScale.Length) //If the player's level is less than the max level
        {
            float Timer = 0f;
            float Duration = 1f;
            float NewXP = CurrentXP + Value;

            while (Timer < Duration)
            {
                Timer += Time.fixedDeltaTime;
                float Step = Timer / Duration;
                CurrentXP = Mathf.Lerp(CurrentXP, NewXP, Step);    
                yield return null;
            }

            if (CurrentXP > XPScale[Level])
            {
                CurrentXP -= XPScale[Level];
                Level += 1;
                XPSlider.maxValue = XPScale[Level];
                #region Upgrade Player Stats
                Damage += 10;
                MaxHealth += 10;
                MaxMana += 10;
                MaxStamina += 10;
                SkillDamage += 5;
                Armor += 5;
                MagicResist += 5;
                #endregion
            }
            LevelText.text = Level.ToString();
        }
        else
        {
            LevelText.text = "Max";
        }
    }
    #endregion

    #region Self Talk
    public void SelfTalk(string Text)
    {
        SelfTalkObject.SetActive(true);
        TWE.fulltext = Text;
        TWE.gameObject.SetActive(true);
        AC.animator.SetFloat("State", 0);
        Freeze();
    }
    #endregion

    #region Buffs
    [Header("For stats")]
    public float Duration;
    public float Increaser;
    private bool DamageStatsOn;
    private bool MagicStatsOn;

    public IEnumerator IncreaseDamageBuff()
    {
        if (!DamageStatsOn)
        {
            DamageStatsOn = true;
            Damage += Increaser;
            Armor += Increaser;
            yield return new WaitForSeconds(Duration);
            Damage -= Increaser;
            Armor -= Increaser;
            DamageStatsOn = false;   
        }
    }

    public IEnumerator IncreaseSpellBuff()
    {
        if (!MagicStatsOn)
        {
            MagicStatsOn = true;
            SkillDamage += Increaser;
            MagicResist += Increaser;
            yield return new WaitForSeconds(Duration);
            SkillDamage -= Increaser;
            MagicResist -= Increaser;
            MagicStatsOn = false;
        }
    }
    #endregion
}
