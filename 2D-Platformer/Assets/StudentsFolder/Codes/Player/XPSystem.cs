using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPSystem : MonoBehaviour
{
    #region Variables
    [Header("XP")]
    public int Scale;
    public int MaxLevel;
    public int[] LevelScaling;

    [Header("Player")]
    public int CurrentXP;
    public int Level;
    public int SkillPoint;
    public PlayerMovement PM;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < MaxLevel; i++)
        {
            LevelScaling[i] = Scale * i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Gain XP 
    public void GainXP(int Value)
    {
        CurrentXP += Value;

        if (CurrentXP > LevelScaling[Level])
        {
            CurrentXP -= LevelScaling[Level];
            Level += 1;
            SkillPoint += 2;
            GainBuff();
        }
    }

    public void GainBuff()
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
