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
    public int[] CarryAmount;

    [Header("Player Inventory")]
    public Image[] SlotImage;
    public TextMeshProUGUI[] AmountText;
    [HideInInspector] public bool[] SlotFull;
    [HideInInspector] public string[] SlotName;
    public int[] SlotAmount;
    public int StackableAmount;
    public Sprite EmptySprite;
    [HideInInspector] public int SlotAvailable;
    [HideInInspector] public bool ItemExists;
    [HideInInspector] public int FoundSlot;

    [Header("Gold")]
    public int Gold;
    public TextMeshProUGUI GoldText;

    [Header("References")]
    public PlayerMovement PM;

    [Space(10)]
    public string RemovableItem;
    #endregion

    #region Start Update
    void Start()
    {
        SlotFull = new bool[SlotImage.Length];
        SlotName = new string[SlotImage.Length];
        SlotAmount = new int[SlotImage.Length];
        SlotAvailable = SlotImage.Length + 1;
    }

    void Update()
    {
        #region Potion Handler

        #endregion

        GoldText.text = Gold.ToString();

        if (Input.GetKeyDown(KeyCode.O))
            RemoveItem(RemovableItem);
    }
    #endregion

    #region Add Item
    public void AddItem(string Name, bool Stackable)
    {
        if (Stackable)
            AddingStackableItem(Name);     
        else
            AddingItem(Name, false);

        RefreshInventoryVariables();
    }

    void AddingStackableItem(string Name)
    {
        for (int i = 0; i < SlotImage.Length; i++)
        {
            if (SlotFull[i] == true)
            {
                if (SlotName[i] == Name && SlotAmount[i] != StackableAmount)
                {
                    SlotAmount[i]++;
                    AmountText[i].text = SlotAmount[i].ToString();

                    if (SlotAmount[i] > StackableAmount)
                    {
                        SlotAmount[i]--;
                        AmountText[i].text = SlotAmount[i].ToString();
                        AddingItem(Name, true);
                    }
                    break;
                }
            }
            else
            {
                AddingItem(Name, true);
                break;
            }
        }
    }

    void AddingItem(string Name, bool Stackable)
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
                        SlotAmount[i] = 1;
                        break;
                    }
                }

                if (Stackable)
                {
                    AmountText[i].gameObject.SetActive(true);
                    AmountText[i].text = "1";
                }
                break;
            }
        }
        RefreshInventoryVariables();
    }
    #endregion

    #region Remove Item
    public void RemoveItem(string Name)
    {
        for (int i = 0; i < SlotImage.Length; i++)
        {
            if (SlotName[i] == Name)
            {
                if (AmountText[i].gameObject.activeSelf)
                {
                    if (SlotAmount[i] > 1)
                    {
                        SlotAmount[i]--;
                        AmountText[i].text = SlotAmount[i].ToString();

                        if (SlotAmount[i] == 0)
                        {
                            AmountText[i].gameObject.SetActive(false);
                            AmountText[i].text = "0";
                            RemovingItem(Name);
                        }
                        break;
                    }
                    else
                    {
                        AmountText[i].gameObject.SetActive(false);
                        AmountText[i].text = "0";
                        RemovingItem(Name);
                        break;
                    }
                }
                else
                {
                    RemovingItem(Name);
                    break;
                }
            }
        }

        RefreshInventoryVariables();
    }

    void RemovingItem(string Name)
    {
        for (int i = 0; i < SlotImage.Length; i++)
        {
            if (SlotName[i] == Name)
            {
                SlotImage[i].sprite = EmptySprite;
                SlotFull[i] = false;
                SlotName[i] = "Empty";
                SlotAvailable++;
                SlotAmount[i] = 0;
                AmountText[i].gameObject.SetActive(false);
                AmountText[i].text = "0";
                break;
            }
        }
        RefreshInventoryVariables();
    }
    #endregion

    #region Check For Item
    public void CheckForItem(string Name)
    {
        ItemExists = false;
        FoundSlot = -1;
        for (int i = 0; i < SlotImage.Length; i++)
        {
            if (SlotName[i] == Name)
            {
                ItemExists = true;
                FoundSlot = i;
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
        SlotAmount[Number] = 0;
        AmountText[Number].gameObject.SetActive(false);
        AmountText[Number].text = "0";
    }
    #endregion

    #region Refresh Inventory & Sorting System
    public void RefreshInventoryVariables()
    {
        CarryAmount = new int[ItemName.Length];
        for (int i = 0; i < CarryAmount.Length; i++)
        {
            for (int j = 0; j < SlotImage.Length; j++)
            {
                if (ItemName[i] == SlotName[j])
                {
                    if (SlotAmount[j] == 0)
                        CarryAmount[i]++;
                    else
                        CarryAmount[i] += SlotAmount[j];
                }
            }
        }
    }

    public void SortingSystem()
    {
        string[] SavedSlotName = new string[SlotImage.Length];
        SavedSlotName = SlotName;
        int[] SavedSlotAmount = new int [SlotImage.Length];
        SavedSlotAmount = SlotAmount;
        
        for (int i = 0; i < SlotImage.Length; i++)
        {
            SlotImage[i].sprite = EmptySprite;
            SlotFull[i] = false;
            SlotName[i] = "Empty";
            SlotAvailable++;
            SlotAmount[i] = 0;
            AmountText[i].gameObject.SetActive(false);
            AmountText[i].text = "0";
        }

        for (int i = 0; i < SlotImage.Length; i++)
        {
            if (SavedSlotAmount[i] > 1)
            {
                for (int j = 0; j < SavedSlotAmount[i]; j++)
                {
                    AddItem(SavedSlotName[i], true);
                }
            }
            else if (SavedSlotAmount[i] == 1)
                AddItem(SavedSlotName[i], false);
        }
    }
    #endregion
}
