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
            OpenOrCloseShop();

        if (Input.GetKeyDown(KeyCode.Escape) && CanInteract && ThisCanvas.activeSelf == true)
            OpenOrCloseShop();

        #region Assign Player's Gold And Item's Amount In UI
        if (ThisCanvas.activeSelf == true) //If the shop is open
        {
            GoldText.text = PM.Gold.ToString(); //Show the player's gold into a text
            for (int i = 0; i < BuyItemAmount.Length; i++)
            {
                BuyItemAmount[i].text = PlayerItemsAmount[i].ToString(); //Show the amount (Buy UI)
                SellItemAmount[i].text = PlayerItemsAmount[i].ToString();//Show the amount (Sell UI)
            }

            for (int i = 0; i < PI_Text.Length; i++)
            {
                PI_Text[i].text = PI_Amount[i].ToString(); //Show amount (Trading items)
                NPCI_Text[i].text = NPCI_Amount[i].ToString();//Show amount (Traded items)
            }
        }
        #endregion
    }
    #endregion

    #region Open Or Close Shop Function
    void OpenOrCloseShop()
    {
        FindTradingAmount();
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
    public int[] NPCItemAmount; //Amount of the new item

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

            for (int i = 0; i < NPCItemAmount[Number]; i++)
            MI.AddItem(NPCItem[Number]);
        }

        FindTradingAmount();
    }
    #endregion

    #region Smelting
    [Header("Smelting")]
    public string[] RequiredOre;
    public string[] SmeltedIngot;
    public int Payment;

    public void Smelting(int Number)
    {
        MI.CheckForItem(RequiredOre[Number]);

        if(MI.ItemExists && PM.Gold >= Payment)
        {
            PM.Gold -= Payment;
            MI.RemoveItem(RequiredOre[Number]);
            MI.AddItem(SmeltedIngot[Number]);
        }
    }
    #endregion

    #region Crafting
    [Header("Crafting")]
    public string[] Ingr0;
    public string[] Ingr1;
    public string[] Ingr2;
    public string[] CraftedItem;

    public void Crafting(int Number)
    {
        bool Check0 = false;
        bool Check1 = false;
        bool Check2 = false;

        #region Check Ingredient 0
        if (Ingr0[Number] != null && Ingr0[Number] != "None" && Ingr0[Number] != "Empty")
        {
            MI.CheckForItem(Ingr0[Number]);
            Check0 = MI.ItemExists;
            MI.RemoveItem(Ingr0[Number]);
        }
        else Check0 = true;
        #endregion
        #region Check Ingredient 1
        if (Ingr1[Number] != null && Ingr1[Number] != "None" && Ingr1[Number] != "Empty")
        {
            MI.CheckForItem(Ingr1[Number]);
            Check1 = MI.ItemExists;
            MI.RemoveItem(Ingr1[Number]);
        }
        else Check1 = true;
        #endregion
        #region Check Ingredient 2
        if (Ingr2[Number] != null && Ingr2[Number] != "None" && Ingr2[Number] != "Empty")
        {
            MI.CheckForItem(Ingr2[Number]);
            Check2 = MI.ItemExists;
            MI.RemoveItem(Ingr2[Number]);
        }
        else Check2 = true;
        #endregion

        if (Check0 && Check1 && Check2)
            MI.AddItem(CraftedItem[Number]);
        else
        {
            if(Check0)
            MI.AddItem(Ingr0[Number]);
            ////////////////////
            if(Check1)
            MI.AddItem(Ingr1[Number]);
            ////////////////////
            if(Check2)
            MI.AddItem(Ingr2[Number]);
            ////////////////////
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
                    break;
                }
            }
        }
    }
    #endregion

    #region Find the trading amount
    [Header("UI Trading")]
    public TextMeshProUGUI[] PI_Text; //Player Item Text
    public TextMeshProUGUI[] NPCI_Text; //NPC Item Text
    private int[] PI_Amount; //Player Item Amount
    private int[] NPCI_Amount; //NPC Item Amount

    void FindTradingAmount() //Player's and NPC
    {
        PI_Amount = new int[PI_Text.Length]; //Create a new empty array with the amounts
        for (int i = 0; i < MI.SlotImage.Length; i++) //Search the whole inventory one by one
            for (int j = 0; j < PlayerItem.Length; j++)
                if (MI.SlotName[i] == PlayerItem[j]) // Check if any item in the inventory matches an item in the other array
                {
                    PI_Amount[j] += 1;
                    break;
                }

        NPCI_Amount = new int[NPCI_Text.Length];
        for (int i = 0; i < MI.SlotImage.Length; i++)
            for (int j = 0; j < NPCItem.Length; j++)
                if (MI.SlotName[i] == NPCItem[j])
                {
                    NPCI_Amount[j] += 1;
                    break;
                }

    }
    #endregion
}
