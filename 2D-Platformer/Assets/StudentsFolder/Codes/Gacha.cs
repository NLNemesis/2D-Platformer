using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    #region Variables
    [Header("Items")]
    public string MainFiveStarWeapon;
    public string[] FiveStarWeapon;
    public string[] FourStarWeapon;
    public string[] ThreeStarWeapon;

    [Header("Player")]
    public int Pity;
    public int CurrentPull;

    [Header("References")]
    public MyInventory MI;
    public PlayerMovement PM;
    #endregion

    public void BuyWish()
    {
        if (PM.Gold >= 1000)
        {
            PM.Gold -= 1000;
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
            CurrentPull++;
            int RandomInt = Random.Range(0, 91);
            //Check if the player can pull a five star weapons
            if (Pity >= RandomInt) //Player pulls a five star weapon
            {
                Pity = 0;
                CurrentPull = 0;
                RandomInt = Random.Range(0, 2);
                if (RandomInt == 0) //Player takes the main five star weapon
                {
                    MI.AddItem(MainFiveStarWeapon);
                }
                else //Player takes random five star weapon (not the main five star)
                {
                    RandomInt = Random.Range(0, FiveStarWeapon.Length);
                    MI.AddItem(FiveStarWeapon[RandomInt]);
                }
            }
            else if (CurrentPull == 10) //if the player wishes 10 times for a weapon
            {
                CurrentPull = 0;
                RandomInt = Random.Range(0, FourStarWeapon.Length);
                MI.AddItem(FourStarWeapon[RandomInt]);
            }
            else //Player takes a three star weapon
            {
                RandomInt = Random.Range(0, ThreeStarWeapon.Length);
                MI.AddItem(ThreeStarWeapon[RandomInt]);
            }
        }
    }
}
