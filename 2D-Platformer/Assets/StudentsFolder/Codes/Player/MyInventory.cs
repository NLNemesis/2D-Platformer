using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInventory : MonoBehaviour
{
    [Header("Inventory")]
    public Image[] SlotImage;
    public string[] SlotName;
    public int SlotAvailable;

    [Header("World Items")]
    public Sprite EmptySprite;
    public Sprite[] ItemSprite;
    public string[] ItemName;

    public void AddItem(string name)
    {
        for (int i = 0; i < SlotName.Length; i++)
        {
            if (SlotImage[i].sprite == EmptySprite)
            {
                SlotName[i] = name;
                SlotAvailable--;
                #region Search Item Image
                for (int j = 0; j < ItemName.Length; j++)
                {
                    if (ItemName[j] == name)
                    {
                        SlotImage[i].sprite = ItemSprite[j];
                        break;
                    }
                }
                #endregion
                break;
            }
        }
    }
}
