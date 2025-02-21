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
    public PlayerMovement PM;

    [Header("Events")]
    public UnityEvent[] AbilityEvent;
    public UnityEvent[] TalentEvent;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check player's level for the abilities
        for (int i = 0; i < Ability.Length; i++)
            if (XPS.Level >= AbilityRestriction[i])
                AbilityUnlocked[i] = true;

        //Check player's level for the talents
        for (int i = 0; i < Talent.Length; i++)
            if (XPS.Level >= TalentRestriction[i])
                TalentUnlocked[i] = true;
    }

    public void GainAbility(int Number)
    {
        if (XPS.AbilityPoint > 0 && AbilityUnlocked[Number] == true)
        {
            XPS.AbilityPoint -= 1;
            Ability[Number] = true;
            AbilityEvent[Number].Invoke();
        }
    }

    public void GainTalent(int Number)
    {
        if (XPS.TalentPoint > 0 && TalentUnlocked[Number] == true)
        {
            XPS.TalentPoint -= 1;
            Talent[Number] = true;
            TalentEvent[Number].Invoke();
        }
    }
}
