using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

public class SpearWomanSkills : MonoBehaviour
{
    #region Variables
    [Header("Transformation")]
    public float ManaReductionDuration;
    public Image TransformationImage;

    [Header("Requirments")]
    public bool[] SpellActive;
    public float[] RequiredMana;
    public float[] CDR;
    public Animator[] CDRAnimator;

    [Header("Jump electric shock")]
    public Transform AttackPoint;
    public float AttackRange;

    [Header("References")]
    private PlayerMovement PM;
    private AnimController AC;
    #endregion

    private void Start()
    {
        PM = GetComponent<PlayerMovement>();
        AC = GetComponent<AnimController>();

        #region Set Animator's speed
        for (int i = 0; i < CDR.Length; i++)
        {
            CDRAnimator[i].speed = 1 / CDR[i];
            if (SpellActive[i])
                CDRAnimator[i].SetTrigger("InstantCDR");
        }
        #endregion
    }

    private void Update()
    {
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

        #region Lighting Ball
        if (Input.GetKeyDown(KeyCode.Alpha2) && PM.Mana >= RequiredMana[1] && SpellActive[1])
            StartCoroutine(LightingBall());
        #endregion

        #region Lighting Spear
        if (Input.GetKeyDown(KeyCode.Alpha3) && PM.Mana >= RequiredMana[2] && SpellActive[2])
            StartCoroutine(MultySpear());
        #endregion

        #region Lighting Strike
        if (Input.GetKeyDown(KeyCode.Alpha5) && PM.Mana >= RequiredMana[4] && SpellActive[4])
            StartCoroutine(LightingStrike());
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

    #region Lighint Ball
    IEnumerator LightingBall()
    {
        PM.Mana -= RequiredMana[1];
        SpellActive[1] = false;
        AC.animator.SetTrigger("LightingBall");
        CDRAnimator[1].SetTrigger("CDR");
        yield return new WaitForSeconds(CDR[1]);
        SpellActive[1] = true;
    }
    #endregion

    #region MultySpear Spell
    IEnumerator MultySpear()
    {
        PM.Mana -= RequiredMana[2];
        SpellActive[2] = false;
        AC.animator.SetTrigger("MultySpear");
        CDRAnimator[2].SetTrigger("CDR");
        yield return new WaitForSeconds(CDR[2]);
        SpellActive[2] = true;
    }
    #endregion

    #region Lighting Strike
    IEnumerator LightingStrike()
    {
        PM.Mana -= RequiredMana[4];
        SpellActive[4] = false;
        AC.animator.SetTrigger("LightingStrike");
        CDRAnimator[4].SetTrigger("CDR");
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

    #region On Draw Gizmos
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint != null)
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
    #endregion
}
