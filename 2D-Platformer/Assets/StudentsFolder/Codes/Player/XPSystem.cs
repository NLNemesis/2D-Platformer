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
    public int[] LevelScaling;

    [Header("Player")]
    public int CurrentXP;
    public int Level;
    public int SkillPoint;
    public PlayerMovement PM;

    [Header("UI")]
    public Slider XPSlider;
    public TextMeshProUGUI LevelText;
    #endregion
    void Awake()
    {
        for (int i = 0; i < LevelScaling.Length; i++)
        {
            LevelScaling[i] = Scale * i;
        }
        XPSlider.maxValue = LevelScaling[1];
    }

    // Update is called once per frame
    void Update()
    {
        XPSlider.value = CurrentXP;
        LevelText.text = Level.ToString();
    }
    #region Gain XP-Buff
    public void GainXP(int Value)
    {
        CurrentXP += Value;
        if (CurrentXP > LevelScaling[Level])
        {
            CurrentXP -= LevelScaling[Level];
            Level += 1;
            SkillPoint += 2;
            XPSlider.maxValue = LevelScaling[Level];
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
    #endregion
}
