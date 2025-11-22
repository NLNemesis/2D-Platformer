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
                break;
            }
        }
    }
    #endregion
}
