using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MyBlacksmith : MonoBehaviour
{
    #region Variables
    [Header("Interaction")]
    public GameObject Message;
    public GameObject ThisCanvas;
    private bool CanInteract;

    [Header("Smelting")]
    public string[] RequiredOre;
    public int[] Payment;
    public string[] SmeltingIngot;

    [Header("Trading")]
    public string[] RequiredItem;
    public int[] ItemAmount;
    public string[] GivenItem;

    [Header("For UI Assignment")]
    public string[] Item;
    public TextMeshProUGUI[] BuyItemAmount;
    public TextMeshProUGUI[] SellItemAmount;
    public TextMeshProUGUI GoldText;
    private int[] PlayerItemsAmount;

    [Header("References")]
    public PlayerMovement PM;
    public MyInventory MI;

    [Header("Events")]
    public UnityEvent OpenShopEvent;
    public UnityEvent CloseShopEvent;
    #endregion

    #region OnTriggers
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && ThisCanvas.activeSelf == false)
        {
            CanInteract = true;
            Message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CanInteract = false;
            Message.SetActive(false);
        }
    }
    #endregion

    #region Start & Update
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanInteract)
        {
            OpenOrCloseShop();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && CanInteract && ThisCanvas.activeSelf == true)
            OpenOrCloseShop();

        #region Assign Player's Gold And Item's Amount In UI
        if (ThisCanvas.activeSelf == true)
        {
            GoldText.text = PM.Gold.ToString();
            for (int i = 0; i < BuyItemAmount.Length; i++)
            {
                BuyItemAmount[i].text = PlayerItemsAmount[i].ToString();
                SellItemAmount[i].text = PlayerItemsAmount[i].ToString();
            }
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.L))
        {
            Trading(0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Trading(1);
        }
    }
    #endregion

    #region Open Or Close Shop Function
    void OpenOrCloseShop()
    {
        if (ThisCanvas.activeSelf == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            OpenShopEvent.Invoke();
            FindItemsAmount();
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CloseShopEvent.Invoke();
        }
    }
    #endregion

    #region Smelting
    public void Smelting(int Number)
    {
        MI.CheckForItem(RequiredOre[Number]);

        if (PM.Gold >= Payment[Number] && MI.ItemExists)
        {
            PM.Gold -= Payment[Number];
            MI.RemoveItem(RequiredOre[Number]);
            MI.AddItem(SmeltingIngot[Number], true);
        }
    }
    #endregion

    #region Trading
    public void Trading(int Number)
    {
        for (int i = 0; i < MI.ItemName.Length; i++) 
        { 
            if (MI.ItemName[i] == RequiredItem[Number] && MI.CarryAmount[i] >= ItemAmount[Number]) 
            {
                for (int j = 0; j < ItemAmount[Number]; j++)
                {
                    MI.RemoveItem(RequiredItem[Number]);
                }
                MI.AddItem(GivenItem[Number], false);
                MI.RefreshInventoryVariables();
                Debug.Log(i);
                //MI.SortingSystem();
                break;
            }
        }
    }
    #endregion

    #region Find the amount of the items
    void FindItemsAmount()
    {
        PlayerItemsAmount = new int[Item.Length];
        for (int i = 0; i < Item.Length; i++)
        {
            for (int j = 0; j < MI.SlotImage.Length; j++)
            {
                if (Item[i] == MI.SlotName[j])
                {
                    if (MI.SlotAmount[j] == 0)
                        PlayerItemsAmount[i] += 1;
                    else
                        PlayerItemsAmount[i] += MI.SlotAmount[j];
                }
            }
        }
    }
    #endregion
}
