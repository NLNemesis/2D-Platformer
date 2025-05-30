using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Alchemist : MonoBehaviour
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

    #region Alchemy
    [Header("Alchemy")]
    public string[] RequiredItem;
    public string[] AlchemyItem;
    public int Payment;

    public void Alchemy(int Number)
    {
        inventory.CheckForItem(RequiredItem[Number]);

        if(inventory.CheckForItem(RequiredItem[Number]) && PM.Gold >= Payment)
        {
            PM.Gold -= Payment;
            inventory.RemoveItem(RequiredItem[Number]);
            inventory.AddItem(AlchemyItem[Number]);
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
            Check1 = Check0 = inventory.CheckForItem(Ingr1[Number]);
            inventory.RemoveItem(Ingr1[Number]);
        }
        else Check1 = true;
        #endregion
        #region Check Ingredient 2
        if (Ingr2[Number] != null && Ingr2[Number] != "None" && Ingr2[Number] != "Empty")
        {
            inventory.CheckForItem(Ingr2[Number]);
            Check2 = Check0 = inventory.CheckForItem(Ingr2[Number]);
            inventory.RemoveItem(Ingr2[Number]);
        }
        else Check2 = true;
        #endregion

        if (Check0 && Check1 && Check2)
            inventory.AddItem(CraftedItem[Number]);
        else
        {
            if (Check0)
                inventory.AddItem(Ingr0[Number]);
            ////////////////////
            if (Check1)
                inventory.AddItem(Ingr1[Number]);
            ////////////////////
            if (Check2)
                inventory.AddItem(Ingr2[Number]);
            ////////////////////
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
                    Debug.Log(Item[j] + " " + PlayerItemsAmount[j]);
                    break;
                }
            }
        }
    }
    #endregion
}
