using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyInventory : MonoBehaviour
{
    #region Variables
    [Header("Game Items")]
    public Sprite[] ItemSprite;
    public string[] ItemName;

    [Header("Player Inventory")]
    public Image[] SlotImage;
    public TextMeshProUGUI[] AmountText;
    [HideInInspector] public bool[] SlotFull;
    [HideInInspector] public string[] SlotName;
    public Sprite EmptySprite;
    [HideInInspector] public int SlotAvailable;
    [HideInInspector] public bool ItemExists;

    [Header("Gold")]
    public int Gold;
    public TextMeshProUGUI GoldText;

    [Header("References")]
    public PlayerMovement PM;
    #endregion

    void Start()
    {
        SlotFull = new bool[SlotImage.Length];
        SlotName = new string[SlotImage.Length];
        SlotAvailable = SlotImage.Length + 1;
    }

    void Update()
    {
        #region Potion Handler

        #endregion

        GoldText.text = Gold.ToString();
    }

    #region Add Item
    public void AddItem(string Name)
    {
        for (int i = 0; i < SlotImage.Length; i++) 
        {
            if (SlotFull[i] == false)
            {
                SlotFull[i] = true;
                SlotName[i] = Name;
                SlotAvailable--;
                for (int j = 0; j < ItemSprite.Length; j++)
                {
                    if (ItemName[j] == Name)
                    {
                        SlotImage[i].sprite = ItemSprite[j];
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
        for (int i = 0; i < SlotImage.Length; i++)
        {
            if (SlotName[i] == Name)
            {
                SlotImage[i].sprite = EmptySprite;
                SlotFull[i] = false;
                SlotName[i] = "Empty";
                SlotAvailable++;
                break;
            }
        }
    }
    #endregion

    #region Check For Item
    public void CheckForItem(string Name)
    {
        ItemExists = false;
        for (int i = 0; i < SlotImage.Length; i++)
        {
            if (SlotName[i] == Name)
            {
                ItemExists = true;
                break;
            }
        }
    }
    #endregion

    #region Use Inventory Item Library
    public void UseItem(int Number)
    {
        switch (SlotName[Number])
        {
            case "Health Potion":
            {   
                PM.Health += 50;
                if (PM.Health > PM.MaxHealth)
                PM.Health = PM.MaxHealth;
                RemoveSpecificSlot(Number);
                break;
            }
            case "Mana Potion":
            {   
                PM.Mana += 50;
                if (PM.Mana > PM.MaxMana)
                PM.Mana = PM.MaxMana;
                RemoveSpecificSlot(Number);
                break;
            }
        }
    }

    void RemoveSpecificSlot(int Number)
    {
        SlotImage[Number].sprite = EmptySprite;
        SlotFull[Number] = false;
        SlotName[Number] = "Empty";
        SlotAvailable++;
    }
    #endregion
}
