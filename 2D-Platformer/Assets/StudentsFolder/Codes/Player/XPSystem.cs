using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class XPSystem : MonoBehaviour
{
    #region Variables
    [Header("XP")]
    public int Scaling;
    public int[] LevelScale;
    public float StatsBonus; 

    [Header("UI Assignment")]
    public Slider XPSlider;
    public TextMeshProUGUI XPLevel;

    [Header("Player")]
    public int Level;
    public int XPPoint;
    private float CurrentXP;
    public PlayerMovement PM;
    #endregion
    void Awake()
    {
        //Set the requirement xp for the levels
        for (int i = 0; i < LevelScale.Length; i++)
            LevelScale[i] = i * Scaling;
    }

    void Update()
    {
        
    }

    #region Gain XP Function
    public void GainXP(float Value)
    {
        CurrentXP += Value;
        if(CurrentXP > LevelScale[Level])
        {
            CurrentXP -= LevelScale[Level];
            Level += 1;
            GainStatsBuff();
        }
    }
    #endregion
    
    #region GainStatsBuff Function
    public void GainStatsBuff()
    {
        XPPoint += 2;
        PM.MaxHealth += StatsBonus;
        PM.MaxMana += StatsBonus; 
        PM.MaxStamina += StatsBonus;
        PM.Damage += StatsBonus;
        PM.SkillDamage += StatsBonus;
        PM.Armor += StatsBonus;
        PM.MagicResist += StatsBonus;
    }
    #endregion
}
