using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Gacha : MonoBehaviour
{
    #region Variables
    [Header("Items")]
    public string MainFiveStar;
    public string[] FiveStar;
    public string[] FourStar;
    public string[] ThreeStar;

    [Header("Wishes")]
    private int Pity;
    private int CurrentPull;
    private bool Guarantee;

    [Header("References")]
    public PlayerMovement PM;
    public MyInventory MI;
    #endregion

    public void BuyWish()
    {
        if (PM.Gold >= 1000 && MI.SlotAvailable > 0)
        {
            PM.Gold -= 1000;
            MI.AddItem("Wish");
        }
    }

    public void UseWish()
    {
        if(MI.SlotAvailable > 0)
        {
            MI.CheckForItem("Wish");
            if (MI.ItemExists == true)
            {
                MI.RemoveItem("Wish");
                Pity += 1;
                CurrentPull += 1;
                //Try to pull the five star weapon
                int Number = Random.Range(0, 91);
                if (Pity >= Number) //Takes the five star weapon
                {
                    Pity = 0;
                    if (Guarantee) //If the player has guarantee the banner's five star weapon
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
                else if (CurrentPull >= 10) //Takes the four star weapon
                {
                    CurrentPull = 0;
                    Number = Random.Range(0, FourStar.Length);
                    MI.AddItem(FourStar[Number]);
                }
                else //Takes the three star weapon
                {
                    Number = Random.Range(0, ThreeStar.Length);
                    MI.AddItem(ThreeStar[Number]);
                }
            }
        }
    }
}
