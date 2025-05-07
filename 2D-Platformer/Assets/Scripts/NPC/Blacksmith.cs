using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Blacksmith : MonoBehaviour
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
    public Inventory inventory;

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
            FindTradingAmount();
            FindSmeltingAmount();
            FindCraftingAmount();
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
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CloseShopEvent.Invoke();
        }
    }
    #endregion

    #region Find the amount of the items
    void FindItemsAmount()
    {
        PlayerItemsAmount = new int[Item.Length];
        for (int i = 0; i < inventory.SlotImage.Length; i++)
        {
            for (int j = 0; j < Item.Length; j++)
            {
                if (inventory.SlotName[i] == Item[j])
                {
                    PlayerItemsAmount[j] += 1;
                    break;
                }
            }
        }
    }
    #endregion\

    #region Trading
    [Header("Trading")]
    public string[] PlayerItem; //Requested item from the player
    public int[] RequestAmount; //Requested the amount
    public string[] NPCItem;    //New player's item
    public int[] NPCItemAmount; //Amount of the new item

    public void Trading(int Number)
    {
        int Amount = 0;

        for (int i = 0; i < inventory.SlotImage.Length; i++)
        {
            if (inventory.SlotName[i] == PlayerItem[Number])
            Amount += 1;
        }  

        if (Amount >= RequestAmount[Number])
        {
            for (int i = 0; i < RequestAmount[Number]; i++)
            inventory.RemoveItem(PlayerItem[Number]);

            for (int i = 0; i < NPCItemAmount[Number]; i++)
            inventory.AddItem(NPCItem[Number]);
        }
    }
    #endregion

    #region Find the trading amount
    [Header("UI Trading")]
    public TextMeshProUGUI[] Trading_PI_Text; //Player Item Text
    public TextMeshProUGUI[] Trading_NPCI_Text; //NPC Item Text
    private int[] Trading_PI_Amount; //Player Item Amount
    private int[] Trading_NPCI_Amount; //NPC Item Amount

    void FindTradingAmount() //Player's and NPC
    {
        Trading_PI_Amount = new int[Trading_PI_Text.Length]; //Create a new empty array with the amounts
        for (int i = 0; i < inventory.SlotImage.Length; i++) //Search the whole inventory one by one
            for (int j = 0; j < PlayerItem.Length; j++)
                if (inventory.SlotName[i] == PlayerItem[j]) // Check if any item in the inventory matches an item in the other array
                {
                    Trading_PI_Amount[j] += 1;
                    break;
                }

        Trading_NPCI_Amount = new int[Trading_NPCI_Text.Length];
        for (int i = 0; i < inventory.SlotImage.Length; i++)
            for (int j = 0; j < NPCItem.Length; j++)
                if (inventory.SlotName[i] == NPCItem[j])
                {
                    Trading_NPCI_Amount[j] += 1;
                    break;
                }

        for (int i = 0; i < Trading_PI_Text.Length; i++)
        {
            Trading_PI_Text[i].text = Trading_PI_Amount[i].ToString(); //Show amount (Trading items)
            Trading_NPCI_Text[i].text = Trading_NPCI_Amount[i].ToString();//Show amount (Traded items)
        }
    }
    #endregion

    #region Smelting
    [Header("Smelting")]
    public string[] RequiredOre;
    public string[] SmeltedIngot;
    public int Payment;

    public void Smelting(int Number)
    {
        if(inventory.CheckForItem(RequiredOre[Number]) && PM.Gold >= Payment)
        {
            PM.Gold -= Payment;
            inventory.RemoveItem(RequiredOre[Number]);
            inventory.AddItem(SmeltedIngot[Number]);
        }
    }
    #endregion

    #region Find the smelting amount
    [Header("UI Smelting")]
    public TextMeshProUGUI[] Smelting_PI_Text; //Player Item Text
    public TextMeshProUGUI[] Smelting_NPCI_Text; //NPC Item Text
    private int[] Smelting_PI_Amount; //Player Item Amount
    private int[] Smelting_NPCI_Amount; //NPC Item Amount

    void FindSmeltingAmount() //Player's and NPC
    {
        Smelting_PI_Amount = new int[Smelting_PI_Text.Length]; //Create a new empty array with the amounts
        for (int i = 0; i < inventory.SlotImage.Length; i++) //Search the whole inventory one by one
            for (int j = 0; j < RequiredOre.Length; j++)
                if (inventory.SlotName[i] == RequiredOre[j]) // Check if any item in the inventory matches an item in the other array
                {
                    Smelting_PI_Amount[j] += 1;
                    break;
                }

        Smelting_NPCI_Amount = new int[Smelting_NPCI_Text.Length];
        for (int i = 0; i < inventory.SlotImage.Length; i++)
            for (int j = 0; j < SmeltedIngot.Length; j++)
                if (inventory.SlotName[i] == SmeltedIngot[j])
                {
                    Smelting_NPCI_Amount[j] += 1;
                    break;
                }

        for (int i = 0; i < Smelting_PI_Text.Length; i++)
        {
            Smelting_PI_Text[i].text = Smelting_PI_Amount[i].ToString(); //Show amount (Trading items)
            Smelting_NPCI_Text[i].text = Smelting_NPCI_Amount[i].ToString();//Show amount (Traded items)
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
            inventory.CheckForItem(Ingr0[Number]);
            Check0 = inventory.CheckForItem(Ingr0[Number]);
            inventory.RemoveItem(Ingr0[Number]);
        }
        else Check0 = true;
        #endregion
        #region Check Ingredient 1
        if (Ingr1[Number] != null && Ingr1[Number] != "None" && Ingr1[Number] != "Empty")
        {
            inventory.CheckForItem(Ingr1[Number]);
            Check1 = inventory.CheckForItem(Ingr1[Number]);
            inventory.RemoveItem(Ingr1[Number]);
        }
        else Check1 = true;
        #endregion
        #region Check Ingredient 2
        if (Ingr2[Number] != null && Ingr2[Number] != "None" && Ingr2[Number] != "Empty")
        {
            inventory.CheckForItem(Ingr2[Number]);
            Check2 = inventory.CheckForItem(Ingr2[Number]);
            inventory.RemoveItem(Ingr2[Number]);
        }
        else Check2 = true;
        #endregion

        if (Check0 && Check1 && Check2)
            inventory.AddItem(CraftedItem[Number]);
        else
        {
            if(Check0)
            inventory.AddItem(Ingr0[Number]);
            ////////////////////
            if(Check1)
            inventory.AddItem(Ingr1[Number]);
            ////////////////////
            if(Check2)
            inventory.AddItem(Ingr2[Number]);
            ////////////////////
        }
    }
    #endregion

    #region Find the crafting amount
    [Header("UI Smelting")]
    public TextMeshProUGUI[] Crafting_PI_Text; //Player Item Text
    public TextMeshProUGUI[] Crafting1_PI_Text; //Player Item Text
    public TextMeshProUGUI[] Crafting2_PI_Text; //Player Item Text
    public TextMeshProUGUI[] Crafting_NPCI_Text; //NPC Item Text
    private int[] Crafting_PI_Amount; //Player Item Amount
    private int[] Crafting1_PI_Amount; //Player Item Amount
    private int[] Crafting2_PI_Amount; //Player Item Amount
    private int[] Crafting_NPCI_Amount; //NPC Item Amount

    void FindCraftingAmount() //Player's and NPC
    {
        #region Check Ingredient 0
        Crafting_PI_Amount = new int[Crafting_PI_Text.Length]; //Create a new empty array with the amounts
        for (int i = 0; i < inventory.SlotImage.Length; i++) //Search the whole inventory one by one
            for (int j = 0; j < Ingr0.Length; j++)
                if (inventory.SlotName[i] == Ingr0[j]) // Check if any item in the inventory matches an item in the other array
                {
                    Crafting_PI_Amount[j] += 1;
                    break;
                }
        #endregion
        #region Check Ingredient 1
        Crafting1_PI_Amount = new int[Crafting1_PI_Text.Length]; //Create a new empty array with the amounts
        for (int i = 0; i < inventory.SlotImage.Length; i++) //Search the whole inventory one by one
            for (int j = 0; j < Ingr1.Length; j++)
                if (inventory.SlotName[i] == Ingr1[j]) // Check if any item in the inventory matches an item in the other array
                {
                    Crafting1_PI_Amount[j] += 1;
                    break;
                }
        #endregion
        #region Check Ingredient 2
        Crafting2_PI_Amount = new int[Crafting2_PI_Text.Length]; //Create a new empty array with the amounts
        for (int i = 0; i < inventory.SlotImage.Length; i++) //Search the whole inventory one by one
            for (int j = 0; j < Ingr2.Length; j++)
                if (inventory.SlotName[i] == Ingr2[j]) // Check if any item in the inventory matches an item in the other array
                {
                    Crafting2_PI_Amount[j] += 1;
                    break;
                }
        #endregion
        #region Check Crafted Item
        Crafting_NPCI_Amount = new int[Crafting_NPCI_Text.Length]; //Create a new empty array with the amounts
        for (int i = 0; i < inventory.SlotImage.Length; i++) //Search the whole inventory one by one
            for (int j = 0; j < CraftedItem.Length; j++)
                if (inventory.SlotName[i] == CraftedItem[j]) // Check if any item in the inventory matches an item in the other array
                {
                    Crafting_NPCI_Amount[j] += 1;
                    break;
                }
        #endregion

        for (int i = 0; i < Smelting_PI_Text.Length; i++)
        {
            Crafting_PI_Text[i].text = Crafting_PI_Amount[i].ToString(); //Show amount (Trading items)
            Crafting1_PI_Text[i].text = Crafting1_PI_Amount[i].ToString();//Show amount (Traded items)
            Crafting2_PI_Text[i].text = Crafting2_PI_Amount[i].ToString(); //Show amount (Trading items)
            Crafting_NPCI_Text[i].text = Crafting_NPCI_Amount[i].ToString();//Show amount (Traded items)
        }
    }
    #endregion
}
