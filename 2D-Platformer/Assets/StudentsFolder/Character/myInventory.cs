using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myInventory : MonoBehaviour
{
    #region Variables
    public string[] slotName;
    public Image[] slotImage;
    public Sprite emptySprite;
    public myPlayer player;

    [Header("Counter")]
    public int coins;
    #endregion

    #region Add/Remove Item
    public void AddItem(Sprite icon, string name)
    {
        for (int i = 0; i < slotName.Length; i++)
        {
            if (slotName[i] == "Empty")
            {
                slotName[i] = name;
                slotImage[i].sprite = icon;
                if (name == "Coin") 
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
                if (name == "Coin")
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
        }
        else if (slotName[id] == "Medium_Health_Potion")
        {
            player.GainHP(5);
            slotName[id] = "Empty";
            slotImage[id].sprite = emptySprite;
        }
        else if (slotName[id] == "Large_Health_Potion")
        {
            player.GainHP(7);
            slotName[id] = "Empty";
            slotImage[id].sprite = emptySprite;
        }
    }
    #endregion
}
