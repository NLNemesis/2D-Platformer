using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

public class SpearWomanSkills : MonoBehaviour
{
    #region Variables
    private bool CanUseSpell = true;
    [Header("Transformation")]
    public float ManaReductionDuration;
    public Image TransformationImage;
    public float IncreasedStats;     

    [Header("Requirments")]
    public bool[] SpellActive;
    public float[] RequiredMana;
    public float[] CDR;
    public Animator[] CDRAnimator;

    [Header("Jump electric shock")]
    public Transform AttackPoint;
    public float AttackRange;

    [Header("UI")]
    public int[] LevelRequirment;
    public Image[] SkilLTreePath;

    [Header("References")]
    private PlayerMovement PM;
    private AnimController AC;

    [Header("SavedVariables")]
    private float OriginalDamage;
    private float OriginalSkillDamage;
    private float OriginalArmor;
    private float OriginalMagicResist;
    #endregion

    private void Start()
    {
        PM = GetComponent<PlayerMovement>();
        AC = GetComponent<AnimController>();

        #region Set Saved Stats
        OriginalDamage = PM.Damage;
        OriginalSkillDamage = PM.SkillDamage;
        OriginalArmor = PM.Armor;
        OriginalMagicResist = PM.MagicResist;
        #endregion
    }

    private void Update()
    {
        if (!CanUseSpell) return;

        #region Transformation
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!AC.animator.GetBool("Transformation_Activity") && PM.Mana >= RequiredMana[0])
            {
                AC.animator.SetBool("Transformation_Activity", true);
                AC.animator.SetTrigger("Transform");
                Reduction = true;
                TransformationImage.color = Color.white;
            }
            else
            {
                AC.animator.SetBool("Transformation_Activity", false);
                AC.animator.SetTrigger("Transform");
                StopCoroutine(Transformed());
                Reduction = false;
                TransformationImage.color = Color.grey;
            }
        }

        if (Reduction && AC.animator.GetBool("Transformation_Activity"))
            StartCoroutine(Transformed());
        #endregion

        //If the player upgrades Sylvane to her last form he can't uses the other spells
        if (SkillBuff[11]) return;

        #region Lighting Ball
        if (Input.GetKeyDown(KeyCode.Alpha2) && SpellActive[1])
            if (SkillBuff[10] && PM.Mana >= RequiredMana[1] * 2)
                StartCoroutine(LightingBall());
            else if (!SkillBuff[10] && PM.Mana >= RequiredMana[1])
                StartCoroutine(LightingBall());
        #endregion

        #region Multy Spear
        if (Input.GetKeyDown(KeyCode.Alpha3) && SpellActive[2])
                if (SkillBuff[9])
                    StartCoroutine(MultySpear());
                else if (PM.Mana >= RequiredMana[2])
                    StartCoroutine(MultySpear());
        #endregion

        #region Lighting Dash
        if (Input.GetKeyDown(KeyCode.Alpha4) && SpellActive[3])
                if (SkillBuff[7])
                    StartCoroutine(DashSpear());
                else if (PM.Mana >= RequiredMana[3])
                    StartCoroutine(DashSpear());
        #endregion

        #region Lighting Strike
        if (Input.GetKeyDown(KeyCode.Alpha5) && SpellActive[4])
                if (SkillBuff[8])
                    StartCoroutine(LightingStrike());
                else if (PM.Mana >= RequiredMana[4])
                    StartCoroutine(LightingStrike());
        #endregion

        #region Change Color Skill Tree Lines
        for (int i = 0; i < LevelRequirment.Length; i++)
        {
            if (PM.Level >= LevelRequirment[i])
                SkilLTreePath[i].color = Color.green;
        }
        #endregion
    }

    #region Transformation Activated
    private bool Reduction;
    IEnumerator Transformed()
    {
        Reduction = false;
        yield return new WaitForSeconds(ManaReductionDuration);
        PM.Mana -= RequiredMana[0];
        if (PM.Mana >= RequiredMana[0])
            Reduction = true;
        else
        {
            AC.animator.SetBool("Transformation_Activity", false);
            AC.animator.SetTrigger("Transform");
            TransformationImage.color = Color.grey;
        }
    }
    #endregion

    #region Transformation Increase & Dicrease
    public void IncreaseTransformationStats()
    {
        if (SkillBuff[0]) // if The player upgraded the first Skill "Transformation"
        {
            float TransformationBuff = 10 + IncreasedStats;
            Debug.Log(TransformationBuff);
            PM.Damage += TransformationBuff;
            PM.SkillDamage += TransformationBuff;
            PM.Armor += TransformationBuff;
            PM.MagicResist += TransformationBuff;
        }
        else
        {
            PM.Damage += IncreasedStats;
            PM.SkillDamage +=IncreasedStats;
            PM.Armor += IncreasedStats;
            PM.MagicResist += IncreasedStats;
        }

        if (SkillBuff[2])
        {
            PM.SkillDamage += 5;
            PM.Armor -= 5;
            PM.MagicResist -= 5;
        }

        if (SkillBuff[3])
        {
            PM.Damage -= 5;
            PM.SkillDamage -= 5;
            PM.Armor += 5;
            PM.MagicResist += 5;
        }

        if (SkillBuff[11])
        {
            PM.Damage += IncreasedStats;
            PM.SkillDamage += IncreasedStats;
            PM.Armor += IncreasedStats;
            PM.MagicResist += IncreasedStats;
        }
    }

    public void DecreaseTransformationStats()
    {
        if (SkillBuff[0])
        {
            float TransformationBuff = 10 + IncreasedStats;
            PM.Damage -= TransformationBuff;
            PM.SkillDamage -= TransformationBuff;
            PM.Armor -= TransformationBuff;
            PM.MagicResist -= TransformationBuff;
        }
        else
        {
            PM.Damage -= IncreasedStats;
            PM.SkillDamage -= IncreasedStats;
            PM.Armor -= IncreasedStats;
            PM.MagicResist -= IncreasedStats;
        }

        if (SkillBuff[2])
        {
            PM.SkillDamage -= 5;
            PM.Armor += 5;
            PM.MagicResist += 5;
        }

        if (SkillBuff[3])
        {
            PM.Damage += 5;
            PM.SkillDamage += 5;
            PM.Armor -= 5;
            PM.MagicResist -= 5;
        }

        if (SkillBuff[11])
        {
            PM.Damage -= IncreasedStats;
            PM.SkillDamage -= IncreasedStats;
            PM.Armor -= IncreasedStats;
            PM.MagicResist -= IncreasedStats;
        }
    }
    #endregion

    #region Lighint Ball
    IEnumerator LightingBall()
    {
        ChangeAnimatorUISpeed(1);
        PM.Mana -= RequiredMana[1];
        SpellActive[1] = false;
        AC.animator.SetTrigger("LightingBall");
        yield return new WaitForSeconds(CDR[1]);
        SpellActive[1] = true;
    }
    #endregion

    #region MultySpear Spell
    IEnumerator MultySpear()
    {
        ChangeAnimatorUISpeed(2);
        if (!SkillBuff[9])
            PM.Mana -= RequiredMana[2];
        SpellActive[2] = false;
        AC.animator.SetTrigger("MultySpear");
        yield return new WaitForSeconds(CDR[2]);
        SpellActive[2] = true;
    }
    #endregion

    #region Dash Spear
    IEnumerator DashSpear()
    {
        ChangeAnimatorUISpeed(3);
        if (!SkillBuff[7])
            PM.Mana -= RequiredMana[3];
        SpellActive[3] = false;
        AC.animator.SetTrigger("DashSpear");
        //Buff
        if (SkillBuff[6])
        {
            PM.Health += 50;
            if (PM.Health > PM.MaxHealth)
                PM.Health = PM.MaxHealth;
        }
        yield return new WaitForSeconds(CDR[3]);
        SpellActive[3] = true;
    }
    #endregion

    #region Lighting Strike
    IEnumerator LightingStrike()
    {
        ChangeAnimatorUISpeed(4);
        if (!SkillBuff[8])
            PM.Mana -= RequiredMana[4];
        SpellActive[4] = false;
        AC.animator.SetTrigger("LightingStrike");
        //Buff
        if (SkillBuff[5])
        {
            PM.Health += 50;
            if (PM.Health > PM.MaxHealth)
                PM.Health = PM.MaxHealth;
        }
        yield return new WaitForSeconds(CDR[4]);
        SpellActive[4] = true;
    }
    #endregion

    #region Spawn Projectile
    [Header("Spawn Throwable")]
    public GameObject ThrowablePrefab;
    public Transform ThrowableStartPoint;

    public void SpawnBall()
    {
        Instantiate(ThrowablePrefab, new Vector2(ThrowableStartPoint.position.x, ThrowableStartPoint.position.y), Quaternion.identity);
    }
    #endregion

    #region Lighting Strike Damage
    public void DealLightingStrike()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, AC.EnemyLayer); //Check for the enemies
        for (int i = 0; i < hit.Length; i++)
        {
            AIMove aiMove = hit[i].GetComponent<AIMove>();
            Enemy enemy = hit[i].GetComponent<Enemy>();
            if (aiMove != null)
                aiMove.TakeDamage(PM.Damage, true);
            else if (enemy != null)
                enemy.TakeDamage(PM.Damage, true);
        }
    }
    #endregion

    #region CDR-UI-Speed
    void ChangeAnimatorUISpeed(int Number)
    {
        CDRAnimator[Number].speed = 1;
        CDRAnimator[Number].SetTrigger("CDR");
        CDRAnimator[Number].speed = 1 / CDR[Number];
    }
    #endregion

    #region On Draw Gizmos
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint != null)
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
    #endregion

    //----------------------------BUFFS-----------------------------//
    #region Variables
    [Space(10)]
    public bool[] SkillBuff;
    #endregion

    public void UpgradeSkills(int Number)
    {
        if (PM.Level >= LevelRequirment[Number])
        SkillBuff[Number] = true;
    }
}
