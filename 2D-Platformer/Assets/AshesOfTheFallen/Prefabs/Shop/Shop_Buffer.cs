using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Shop_Buffer : MonoBehaviour
{
    #region Variables
    private bool close;

    public UnityEvent openShopEvent;
    public UnityEvent closeShopEvent;

    [Header("References")]
    public GameObject shopUI;
    private myAnimator playerAnimator;
    private myInventory inventory;
    private GameObject message;
    public TextMeshProUGUI soulsText;

    public int attackPowerPrice;
    public TextMeshProUGUI attackPowerPriceText;
    public TextMeshProUGUI attackPowerText;

    public int dodgePrice;
    public TextMeshProUGUI dodgePriceText;
    public TextMeshProUGUI dodgeText;
    #endregion

    #region On Triggers
    private void OnTriggerEnter2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            close = true;
            message = Object.transform.root.GetComponentInChildren<myGameManager>().interaction_Message;
            message.SetActive(true);
            inventory = Object.GetComponent<myInventory>();
            playerAnimator = Object.GetComponent<myAnimator>();
        }
    }

    private void OnTriggerExit2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            close = false;
            message.SetActive(false);
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (shopUI.activeSelf)
        {
            soulsText.text = inventory.soulEssence.ToString();
            attackPowerPriceText.text = attackPowerPrice.ToString();
            dodgePriceText.text = dodgePrice.ToString();
            attackPowerText.text = playerAnimator.attackPower.ToString();
            dodgeText.text = playerAnimator.dodge.ToString() + "%";
        }

        Handle_Open_Close_Shop();
    }

    #region Handle Open/Close Shop
    void Handle_Open_Close_Shop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (close)
            {
                openShopEvent.Invoke();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (shopUI.activeSelf)
            {
                closeShopEvent.Invoke();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && shopUI.activeSelf)
        {
            closeShopEvent.Invoke();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void CloseShop()
    {
        closeShopEvent.Invoke();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Upgrade Stats
    public void increaseAttackPower()
    {
        if (inventory.soulEssence >= attackPowerPrice)
        {
            inventory.soulEssence -= attackPowerPrice;
            playerAnimator.attackPower += 1;
            attackPowerPrice += 25;
        }
    }

    public void increaseDodge()
    {
        if (inventory.soulEssence >= dodgePrice)
        {
            inventory.soulEssence -= dodgePrice;
            playerAnimator.dodge += 0.5f;
            dodgePrice += 25;
        }
    }
    #endregion
}
