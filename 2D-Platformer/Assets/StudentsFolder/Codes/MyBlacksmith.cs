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

    [Header("Shop")]
    public string[] Item;
    public int[] Value;
    public bool[] Stackable;

    [Header("For UI Assignment")]
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

    #region Trading
    [Header("Trading")]
    public string[] PlayerItem; //Requested item from the player
    public int[] RequestAmount; //Requested the amount
    public string[] NPCItem;    //New player's item

    public void Trading(int Number)
    {
        int Amount = 0;

        for (int i = 0; i < MI.SlotImage.Length; i++)
        {
            if (MI.SlotName[i] == PlayerItem[Number])
            Amount += 1;
        }  

        if (Amount >= RequestAmount[Number])
        {
            for (int i = 0; i < RequestAmount[Number]; i++)
            MI.RemoveItem(PlayerItem[Number]);

            MI.AddItem(NPCItem[Number]);
        }
    }
    #endregion

    #region Crafting
    public string[] Ingr;
    public string[] Ingr1;
    public string[] Ingr2;
    public string[] CraftedItem;

    public void Crafting(int Number)
    {
        bool Check = false;
        bool Check1 = false;
        bool Check2 = false;

        MI.CheckForItem(Ingr[Number]);
        Check = MI.ItemExists;
        MI.CheckForItem(Ingr1[Number]);
        Check1 = MI.ItemExists;
        MI.CheckForItem(Ingr2[Number]);
        Check2 = MI.ItemExists;

        if (Check && Check1 && Check2)
        {
            MI.RemoveItem(Ingr[Number]);
            MI.RemoveItem(Ingr1[Number]);
            MI.RemoveItem(Ingr2[Number]);
            MI.AddItem(CraftedItem[Number]);
        }
    }
    #endregion

    #region Find the amount of the items
    void FindItemsAmount()
    {
        PlayerItemsAmount = new int[Item.Length];
        for (int i = 0; i < MI.SlotImage.Length; i++)
        {
            for (int j = 0; j < Item.Length; j++)
            {
                if (MI.SlotName[i] == Item[j])
                {
                    PlayerItemsAmount[j] += 1;
                    Debug.Log(Item[j] + " " + PlayerItemsAmount[j]);
                    break;
                }
            }
        }
    }
    #endregion
}
