using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Shop : MonoBehaviour
{
    #region Variables
    private bool close;
    public GameObject message;

    public UnityEvent openShopEvent;
    public UnityEvent closeShopEvent;

    [Header("References")]
    public GameObject shopUI;
    public myInventory inventory;
    public TextMeshProUGUI coinsText;
    #endregion

    private void OnTriggerEnter2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            close = true;
            message.SetActive(true);
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

    // Update is called once per frame
    void Update()
    {
        coinsText.text = inventory.coins.ToString();

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
    #endregion

    #region Buy Item
    public string[] itemName;
    public Sprite[] itemIcon;
    public int[] itemCost;

    public void BuyItem(int id)
    {
        if (inventory.coins >= itemCost[id])
        {
            for (int i = 0; i < itemCost[id]; i++)
                inventory.RemoveItem("Coin");
            inventory.AddItem(itemIcon[id], itemName[id]);
        }
    }
    #endregion
}
