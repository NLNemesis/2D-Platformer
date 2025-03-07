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
    #endregion

    public void BuyWish()
    {
        if (MI.SlotAvailable > 0 && MI.Gold >= WishCost)
        {

        }
    }
}
