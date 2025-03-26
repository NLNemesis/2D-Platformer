using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Gacha : MonoBehaviour
{
    #region Variables
    [Header("Wish Rewards")]
    public string MainFiveStar;
    public string[] FiveStar;
    public string[] FourStar;
    public string[] ThreeStar;

    [Header("Controller")]
    public int WishCost;
    private int Pity;
    private int CurrentPity;
    private bool Guarantee;

    [Header("References")]
    public PlayerMovement PM;
    public MyInventory MI;
    public TextMeshProUGUI[] UIText;

    private bool CanCountWishes = true;
    #endregion

    void Update()
    {
        UIText[0].text = MI.Gold.ToString();
        UIText[1].text = WishCost.ToString();
        if(CanCountWishes) CountWishes();
    }

    void CountWishes()
    {
        CanCountWishes = false;
        int Number = 0;
        for (int i = 0; i < MI.SlotName.Length; i++)
            if (MI.SlotName[i] == "Wish") 
                Number++;
        UIText[2].text = Number.ToString();
        CanCountWishes = true;
    }
    #region Wish System
    public void BuyWish()
    {
        if (MI.SlotAvailable > 0 && PM.Gold >= WishCost)
        {
            PM.Gold -= WishCost;
            MI.AddItem("Wish");
        }
    }

    public void Wish()
    {
        MI.CheckForItem("Wish");
        if (MI.ItemExists)
        {
            MI.RemoveItem("Wish");
            Pity++;
            CurrentPity++;
            int Number = Random.Range(0, 91);
            if (Pity >= Number)
            {
                Pity = 0;
                if (Guarantee)
                {
                    Guarantee = false;
                    MI.AddItem(MainFiveStar);
                }
                else
                {
                    Number = Random.Range(0, 2);
                    if (Number == 0)
                        MI.AddItem(MainFiveStar);
                    else
                    {
                        Guarantee = true;
                        Number = Random.Range(0, FiveStar.Length);
                        MI.AddItem(FiveStar[Number]);
                    }
                }
            }
            else if (CurrentPity >= 10)
            {
                CurrentPity = 0;
                Number = Random.Range(0, FourStar.Length);
                MI.AddItem(FourStar[Number]);
            }
            else
            {
                Number = Random.Range(0, ThreeStar.Length);
                MI.AddItem(ThreeStar[Number]);
            }
        }
    }
    #endregion
}
