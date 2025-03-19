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
    [HideInInspector] public bool PlayerFreeze;
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
    public bool CanSlide;
    public float SlideStaminaRequirment;
    private bool IsSliding;
    public float SlidePower = 24;
    public float SlideTimer = 0.2f;
    public float SlideCooldown = 1f;

    [Header("Stats")]
    public bool HasWeapon;
    [HideInInspector] public bool CanAttack;
    public float Damage;
    public float SkillDamage;
    public float Armor;
    public float MagicResist;
    [HideInInspector] public float Attack;

    [Header("Gathering Tier")]
    public int AxeTier;
    public int PickaxeTier;
    public int KnifeTier;

    [Header("References")]
    private Rigidbody2D rb;
    private InputManager IM;
    private UIController UIC;
    [HideInInspector] public AnimController animController;
    public Transform GroundCheck;
    public LayerMask GroundLayer;

    [Header("UI")]
    public Slider HealthSlider;
    public Slider ManaSlider;
    public Slider StaminaSlider;
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
        CanAttack = true;
        OriginalSpeed = Speed;
        #region Assign XP Scale
        float Value = 0;
        for (int i = 0; i < XPScale.Length; i++)
        {
            Value += 250;
            XPScale[i] = Value;
        }
        XPSlider.maxValue = XPScale[0];
        #endregion
    }

    private void Update()
    {
        if (PlayerFreeze == false && Health > 0 && State == 0)
        {
            #region Player Inputs
            if (PlayerFreeze == false)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");

                if (InAction == false)
                {
                    #region Jumping
                    if (Input.GetKeyDown(IM.Jump) && Stamina >= 15 && IsGrounded())
                    {
                        //InAction = true;
                        animController.animator.SetTrigger("Jump");
                        animController.animator.SetBool("InAir", true);
                        Jump = true;
                        Stamina -= 15f;
                        rb.velocity = new Vector2(rb.velocity.x, JumpingPower);
                    }

                    if (Input.GetKeyUp(IM.Jump) && rb.velocity.y > 0f)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                    }

                    if (rb.velocity.y == 0)
                    {
                        Event[1].Invoke();
                    }
                    else
                    {
                        Event[0].Invoke();
                    }
                    #endregion

                    #region Dash
                    if (Input.GetKeyDown(IM.Dash) && Stamina >= 15 && CanDash == true)
                    {
                        Stamina -= DashStaminaRequirment;
                        StartCoroutine(Dash());
                    }
                    #endregion

                    #region Slide
                    if (Input.GetKeyDown(IM.Slide) && Stamina >= 15 && CanSlide == true)
                    {
                        Stamina -= SlideStaminaRequirment;
                        StartCoroutine(Slide());
                    }
                    #endregion

                    #region Light and Heavy Attack
                    if (Input.GetMouseButtonDown(0) && Stamina >= 30 && UIC.InUI == false)
                    {
                        Stamina -= 30f;
                        animController.animator.SetTrigger("Light Attack");
                        FreezePlayer();
                    }

                    if (Input.GetMouseButtonDown(1) && Stamina >= 30 && UIC.InUI == false)
                    {
                        Stamina -= 30f;
                        animController.animator.SetTrigger("Heavy Attack");
                        FreezePlayer();
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
            {
                if (Stamina < MaxStamina && CanIncrease == true)
                {
                    StartCoroutine(StaminaIncrease());
                }
            }
            else
            {
                StopCoroutine(StaminaIncrease());
            }

            if (Stamina > MaxStamina)
            {
                Stamina = MaxStamina;
            }
            #endregion
        }

        #region Assing UI
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = Health;
        ManaSlider.maxValue = MaxMana;
        ManaSlider.value = Mana;
        StaminaSlider.maxValue = MaxStamina;
        StaminaSlider.value = Stamina;
        XPSlider.value = CurrentXP;
        LevelText.text = Level.ToString();
        UIText[3].text = Gold.ToString();
        #endregion
    }

    #region Move The Player
    private void FixedUpdate()
    {
        if (State == 0)
        {
            //Move the player
            if (PlayerFreeze == false)
            {
                rb.velocity = new Vector2(horizontal * Speed, rb.velocity.y);
            }

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
    IEnumerator Dash()
    {
        PlayerFreeze = true;
        CanIncrease = false;
        InAction = true;
        animController.animator.SetTrigger("Dash");
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
        PlayerFreeze = true;
        CanIncrease = false;
        InAction = true;
        animController.animator.SetTrigger("Slide");
        CanSlide = false;
        IsSliding = true;
        float OriginalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * SlidePower, 0f);
        yield return new WaitForSeconds(SlideTimer);
        rb.gravityScale = OriginalGravity;
        IsSliding = false;
        MovementReset();
        yield return new WaitForSeconds(SlideCooldown);
        CanSlide = true;
    }
    #endregion

    #region Resets
    public void FreezePlayer()
    {
        CanAttack = false;
        PlayerFreeze = true;
        InAction = true;
        Speed = 0;
    }

    public void UnFreezePlayer()
    {
        CanAttack = true;
        PlayerFreeze = false;
        InAction = false;
        Speed = OriginalSpeed;
    }

    public void MovementReset()
    {
        CanIncrease = true;
        animController.animator.SetFloat("State", 0);
        Speed = OriginalSpeed;
        InAction = false;
        PlayerFreeze = false;
        CanAttack = true;
    }

    public void PlayerState()
    {
        if (State == 0)
        {
            State = 1;
            Speed = 0;
            animController.animator.SetFloat("State", 0);
        }
        else
        {
            State = 0;
            Speed = OriginalSpeed;
        }
    }
    #endregion

    #region Stamina Lerp
    IEnumerator StaminaIncrease()
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

    #region HealAndManaRegend
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
        FreezePlayer();
        animController.animator.SetTrigger("Hit");

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
            PlayerFreeze = true;
            animController.animator.SetTrigger("Death");
            Speed = 0;
        }
    }
    #endregion

    #region Ladder Controller
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            InLadder = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            InLadder = true;
        }
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
        animController.animator.SetBool("InAir", false);
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
        FreezePlayer();
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
