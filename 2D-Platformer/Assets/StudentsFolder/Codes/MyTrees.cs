using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyTrees : MonoBehaviour
{
    #region Variables
    [Header("UI")]
    public TextMeshProUGUI[] InfoText; //0 = Talent points number, 1 = Ability points number

    [Header("Skill & Talent Tree")]
    public bool[] UnlockedAbility;
    public int[] AbilityRestriction;
    public Image[] AbilityConnects;
    [Space(10)]
    public bool[] UnlockedTalent;
    public int[] TalentRestriction;

    [Header("References")]
    private XPSystem XPS;
    private PlayerMovement PM;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        XPS = GetComponent<XPSystem>();
        PM = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        #region UI Assignment
        InfoText[0].text = XPS.TalentPoint.ToString();
        InfoText[1].text = XPS.AbilityPoint.ToString();

        for (int i = 0; i < AbilityConnects.Length; i++)
        {
            if(XPS.Level >= AbilityRestriction[i])
                AbilityConnects[i].color = new Color(0,100,0,255);
        }
        #endregion
    }

    public void Unlock_Ability(int Number)
    {
        if (XPS.Level >= AbilityRestriction[Number] && !UnlockedAbility[Number])
        {
            UnlockedAbility[Number] = true;
            XPS.AbilityPoint -= 1;
        }
    }

    public void Unlock_Talent(int Number)
    {
        if (XPS.Level >= TalentRestriction[Number] && !UnlockedTalent[Number])
        {
            UnlockedTalent[Number] = true;
            XPS.TalentPoint -= 1;
        }
    }
}
