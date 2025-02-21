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
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        XPS = GetComponent<XPSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
