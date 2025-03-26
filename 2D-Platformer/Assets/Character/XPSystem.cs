using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPSystem : MonoBehaviour
{
    #region Variables
    [Header("XP")]
    public int Scale;
    public int[] LevelScale;

    [Header("Player")]
    public int CurrentXP;
    public int Level;
    public int AbilityPoint;
    public int TalentPoint;
    public PlayerMovement PM;

    [Header("UI")]
    public Slider XPBar;
    public TextMeshProUGUI LevelText;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < LevelScale.Length; i++)
            LevelScale[i] = Scale * i;
    }

    // Update is called once per frame
    void Update()
    {
        XPBar.maxValue = LevelScale[Level];
        XPBar.value = CurrentXP;
        LevelText.text = Level.ToString();
    }

    public void GainXP(int Value)
    {
        CurrentXP += Value;

        if (CurrentXP > LevelScale[Level])
        {
            CurrentXP -= LevelScale[Level];
            Level += 1;
            AbilityPoint += 1;
            TalentPoint += 2;
            GainStatsBuff();
        }
    }

    public void GainStatsBuff()
    {
        PM.MaxHealth += 10;
        PM.MaxMana += 10;
        PM.MaxStamina += 10;
        PM.Damage += 2f;
        PM.SkillDamage += 2f;
        PM.Armor += 2f;
        PM.MagicResist += 2f;
    }
}
