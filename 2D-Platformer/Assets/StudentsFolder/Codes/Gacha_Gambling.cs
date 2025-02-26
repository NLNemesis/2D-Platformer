using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Gacha_Gambling : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement PM;
    public MyInventory MI;

    #region Variables (Gacha)
    [Header("Gacha")]
    public int PullCost;
    public int Pity;
    public string MainFiveStarWeapon;
    public string[] FiveStarWeapon;
    public string[] FourStarWeapon;
    public string[] ThreeStarWeapon;
    private int CurrentPull;
    #endregion

    #region Gacha Functions
    public void BuyPull()
    {
        if (PM.Gold >= PullCost)
        {
            PM.Gold -= PullCost;
            MI.AddItem("Wish");
        }
    }

    public void Pull()
    {
        MI.CheckForItem("Wish");
        if (!MI.ItemExists)
            return;

        MI.RemoveItem("Wish");

        Pity++;
        CurrentPull++;
        int RandomInt = Random.Range(0, 101);
        if (Pity >= RandomInt)
        {
            Pity = 0;
            CurrentPull = 0;
            RandomInt = Random.Range(0, 2);
            if(RandomInt == 1)
                MI.AddItem(MainFiveStarWeapon);
            else
            {
                RandomInt = Random.Range(0, FiveStarWeapon.Length);
                MI.AddItem(FiveStarWeapon[RandomInt]);
            }
        }
        else if (CurrentPull == 10)
        {
            RandomInt = Random.Range(0, FourStarWeapon.Length);
            MI.AddItem(FourStarWeapon[RandomInt]);
        }
        else
        {
            RandomInt = Random.Range(0, ThreeStarWeapon.Length);
            MI.AddItem(ThreeStarWeapon[RandomInt]);
        }
    }
    #endregion

    #region Variables (Roulette)
    public string[] RouletteReward;
    public Animator RouletteAnimator;
    #endregion

    #region Roulette Function
    public void CallRoulette(int Cost)
    {
        StartCoroutine(Roulette(Cost));
    }

    IEnumerator Roulette(int Cost)
    {
        if (PM.Gold < Cost)
            yield return null;

        PM.Gold -= Cost;
        int RandomInt = Random.Range(0, 6);
        RouletteAnimator.SetFloat("Reward", RandomInt);
        yield return new WaitForSeconds(2f);
        MI.AddItem(RouletteReward[RandomInt]);
    }
    #endregion
}
