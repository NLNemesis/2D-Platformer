using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Trees : MonoBehaviour
{
    #region Variables
    [Header("Ability Tree")]
    public bool[] Ability;
    public bool[] AbilityUnlocked;
    public int[] AbilityRestriction;

    [Header("Talent Tree")]
    public bool[] Talent;
    public bool[] TalentUnlocked;
    public int[] TalentRestriction;

    [Header("References")]
    public XPSystem XPS;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region Restriction Handler
        for (int i = 0; i < Ability.Length; i++) 
            if (XPS.Level >= AbilityRestriction[i]) 
                AbilityUnlocked[i] = true;

        for (int i = 0; i < Talent.Length; i++) 
            if (XPS.Level >= TalentRestriction[i]) 
                TalentUnlocked[i] = true;
        #endregion
    }

    #region Unlock Abilities & Talents
    public void UnlockAbility(int Number)
    {
        if (Ability[Number] == false && AbilityUnlocked[Number] == true && XPS.AbilityPoint > 0)
        {
            Ability[Number] = true;
            XPS.AbilityPoint--;
        }
    }

    public void UnlockTalent(int Number)
    {
        if (Talent[Number] == false && TalentUnlocked[Number] == true && XPS.TalentPoint > 0)
        {
            Talent[Number] = true;
            XPS.TalentPoint--;
        }
    }
    #endregion
}
