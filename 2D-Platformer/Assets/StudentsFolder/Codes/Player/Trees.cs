using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Trees : MonoBehaviour
{
    #region Variables
    [Header("UI")]
    public Image[] AbilityConnect;
    public Image[] AbilityImage;
    public Image[] TalentImage;
    public TextMeshProUGUI[] UIText; // 0 = Ability point text, 1 = Talent point text

    [Header("Ability Tree")]
    public bool[] Ability;
    public bool[] AbilityUnlocked;
    public int[] AbilityRestriction;

    [Header("Talent Tree")]
    public bool[] Talent;
    public bool[] TalentUnlocked;
    public int[] TalentRestriction;

    [Header("References")]
    private XPSystem XPS;

    [Header("Unity Event")]
    public UnityEvent[] AbilityEvent;
    public UnityEvent[] TalentEvent;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        XPS = GetComponent<XPSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        #region  Assign UI
        UIText[0].text = XPS.AbilityPoint.ToString();
        UIText[1].text = XPS.TalentPoint.ToString();
        #endregion

        #region Check restriction
        for (int i = 0; i < Ability.Length; i++)
        {
            if (XPS.Level >= AbilityRestriction[i])
            {
                AbilityUnlocked[i] = true;
                AbilityConnect[i].color = new Color(0, 100, 0, 255); 
            }
        }

        for (int i = 0; i < Talent.Length; i++)
            if (XPS.Level >= TalentRestriction[i])
                TalentUnlocked[i] = true;
        #endregion
    }
    
    #region Gain Abilities & Talents
    public void GainAbility(int Number)
    {
        if (Ability[Number] == false && AbilityUnlocked[Number] && XPS.AbilityPoint > 0)
        {
            XPS.AbilityPoint--;
            Ability[Number] = true;
            AbilityImage[Number].color = new Color(255, 255, 255, 255);
        }
    }

    public void GainTalent(int Number)
    {
        if (Talent[Number] == false && TalentUnlocked[Number] && XPS.TalentPoint > 0)
        {
            XPS.TalentPoint--;
            Talent[Number] = true;
            TalentImage[Number].color = new Color(255, 255, 255, 255);
        }
    }
    #endregion
}
