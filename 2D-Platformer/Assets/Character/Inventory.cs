using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Variables
    [HideInInspector] public bool Check;

    [Header("Inventory Variables")]
    public int Gold;
    public Image[] SlotImage;
    [HideInInspector] public string[] SlotName;
    [HideInInspector] public bool[] SlotFull;
    [HideInInspector] public int SlotAvailable;
    public Sprite EmptySprite;

    [Header("References")]
    private PlayerMovement PM;
    private Animator CanvasAnimator;

    [Header("Game Items")]
    public Sprite[] ItemImage;
    public string[] ItemName;
    #endregion

    #region Start & Update
    private void Start()
    {
        PM = GetComponent<PlayerMovement>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
        SlotName = new string[SlotImage.Length];
        SlotFull = new bool[SlotImage.Length];
        SlotAvailable = SlotImage.Length;
    }
    #endregion

    #region Add Item
    public void AddItem(string Name)
    {
        for (int i = 0; i < SlotFull.Length; i++)
        {
            if (SlotFull[i] == false)
            {
                for (int j = 0; j < ItemImage.Length; j++)
                {
                    if (ItemName[j] == Name)
                    {
                        SlotAvailable -= 1;
                        SlotFull[i] = true;
                        SlotName[i] = Name;
                        SlotImage[i].sprite = ItemImage[j];
                        break;
                    }
                }
                break;
            }
        }
    }
    #endregion

    #region Remove Item
    public void RemoveItem(string Name)
    {
        for (int i = 0; i < SlotName.Length; i++)
        {
            if (SlotName[i] == Name && SlotFull[i] == true)
            {
                SlotAvailable += 1;
                SlotFull[i] = false;
                SlotName[i] = "Empty";
                SlotImage[i].sprite = EmptySprite;
                break;
            }
        }
    }
    #endregion

    #region Select Item Library
    public void Button(int Number)
    {
        if (SlotName[Number] == "Small Health Potion" && PM.Health < PM.MaxHealth)
        {
            StartCoroutine(PM.StatsRegend(25, true));
            #region Remove this item
            SlotAvailable += 1;
            SlotFull[Number] = false;
            SlotName[Number] = "Empty";
            SlotImage[Number].sprite = EmptySprite;
            #endregion
        }
        else if (SlotName[Number] == "Medium Health Potion" && PM.Health < PM.MaxHealth)
        {
            StartCoroutine(PM.StatsRegend(50, true));
            #region Remove this item
            SlotAvailable += 1;
            SlotFull[Number] = false;
            SlotName[Number] = "Empty";
            SlotImage[Number].sprite = EmptySprite;
            #endregion
        }
        else if (SlotName[Number] == "Big Health Potion" && PM.Health < PM.MaxHealth)
        {
            StartCoroutine(PM.StatsRegend(100, true));
            #region Remove this item
            SlotAvailable += 1;
            SlotFull[Number] = false;
            SlotName[Number] = "Empty";
            SlotImage[Number].sprite = EmptySprite;
            #endregion
        }
        else if (SlotName[Number] == "Small Mana Potion" && PM.Mana < PM.MaxMana)
        {
            StartCoroutine(PM.StatsRegend(25, false));
            #region Remove this item
            SlotAvailable += 1;
            SlotFull[Number] = false;
            SlotName[Number] = "Empty";
            SlotImage[Number].sprite = EmptySprite;
            #endregion
        }
        else if (SlotName[Number] == "Medium Mana Potion" && PM.Mana < PM.MaxMana)
        {
            StartCoroutine(PM.StatsRegend(50, false));
            #region Remove this item
            SlotAvailable += 1;
            SlotFull[Number] = false;
            SlotName[Number] = "Empty";
            SlotImage[Number].sprite = EmptySprite;
            #endregion
        }
        else if (SlotName[Number] == "Big Mana Potion" && PM.Mana < PM.MaxMana)
        {
            StartCoroutine(PM.StatsRegend(100, false));
            #region Remove this item
            SlotAvailable += 1;
            SlotFull[Number] = false;
            SlotName[Number] = "Empty";
            SlotImage[Number].sprite = EmptySprite;
            #endregion
        }
    }
    #endregion

    #region Check For Item
    public bool CheckForItem(string Name)
    {
        Check = true;
        for (int i = 0; i < SlotName.Length; i++) 
        { 
            if (SlotName[i] == Name)
            {
                Check = true;
                break;
            }
            else
            {
                Check = false;
            }
        }
        return Check;
    }
    #endregion
}
