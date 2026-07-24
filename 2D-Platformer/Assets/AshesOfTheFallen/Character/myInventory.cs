using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class myInventory : MonoBehaviour
{
    #region Variables
    [Header("Displayment")]
    public TextMeshProUGUI soulEssenceText;

    [Header("Inventory")]
    public string[] slotName;
    public Image[] slotImage;
    public Sprite emptySprite;
    public myPlayer player;
    public int slotAvailable;

    [Header("Counter")]
    public int soulEssence;
    public int coins;

    [Header("References")]
    public AudioSource healSource;
    #endregion

    private void Update()
    {
        soulEssenceText.text = soulEssence.ToString();
    }

    #region Add/Remove Item
    public void AddItem(Sprite icon, string name)
    {
        for (int i = 0; i < slotName.Length; i++)
        {
            if (slotName[i] == "Empty")
            {
                slotName[i] = name;
                slotImage[i].sprite = icon;
                slotAvailable--;
                if (name == "Coin" || name == "Golden Coin") 
                    coins++;
                break;
            }
        }
    }

    public void RemoveItem(string name)
    {
        for (int i = 0; i < slotName.Length; i++)
        {
            if (slotName[i] == name)
            {
                slotName[i] = "Empty";
                slotImage[i].sprite = emptySprite;
                slotAvailable++;
                if (name == "Coin" || name == "Golden Coin")
                    coins--;
                break;
            }
        }
    }
    #endregion

    #region Use Item
    public void UseSlot(int id)
    {
        if (slotName[id] == "Small_Health_Potion")
        {
            player.GainHP(3);
            slotName[id] = "Empty";
            slotImage[id].sprite = emptySprite;
            slotAvailable++;
            healSource.Play();
        }
        else if (slotName[id] == "Medium_Health_Potion")
        {
            player.GainHP(5);
            slotName[id] = "Empty";
            slotImage[id].sprite = emptySprite;
            slotAvailable++;
            healSource.Play();
        }
        else if (slotName[id] == "Large_Health_Potion")
        {
            player.GainHP(7);
            slotName[id] = "Empty";
            slotImage[id].sprite = emptySprite;
            slotAvailable++;
            healSource.Play();
        }
    }
    #endregion

    #region Check For Item
    public bool CheckForItem(string Name)
    {
        bool Check = true;
        for (int i = 0; i < slotName.Length; i++)
        {
            if (slotName[i] == Name)
            {
                Check = true;
                break;
            }
            else
                Check = false;
        }
        return Check;
    }
    #endregion

    #region Load Inventory
    public string[] itemName;
    public Sprite[] itemImage;
    public void LoadInventory()
    {
        for (int i = 0; i < slotName.Length; i++)
        {
            for (int j = 0; j < itemName.Length; j++)
            {
                if (slotName[i] == itemName[j])
                {
                    slotAvailable--;
                    slotImage[i].sprite = itemImage[j];
                    break;
                }
            }
        }
    }
    #endregion
}
