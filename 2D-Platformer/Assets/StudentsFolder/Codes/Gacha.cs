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
    public MyInventory MI;
    public TextMeshProUGUI[] UIText;
    #endregion

    void Update()
    {

    }

    void CountWishes()
    {

    }
    #region Wish System
    public void BuyWish()
    {
        if (MI.SlotAvailable > 0 && MI.Gold >= WishCost)
        {
            MI.Gold -= WishCost;
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
