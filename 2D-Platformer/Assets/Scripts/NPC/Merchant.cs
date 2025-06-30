using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Merchant : MonoBehaviour
{
    #region Variables
    [Header("Interaction")]
    public GameObject Message;
    public GameObject ThisCanvas;
    private bool CanInteract;

    [Header("General Shop")]
    public string[] Item;
    public int[] Value;

    [Header("For UI Assignment")]
    public TextMeshProUGUI[] BuyItemAmount;
    public TextMeshProUGUI[] SellItemAmount;
    public TextMeshProUGUI GoldText;
    private int[] PlayerItemsAmount;

    [Space(10)]
    [Header("Equipment Shop")]
    public string[] Equipment_Item;
    public int[] Equipment_Value;

    [Space(10)]
    [Header("References")]
    private PlayerMovement PM;
    private Inventory inventory;
    private GameController GC;

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
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        inventory = GameObject.Find("/MaxPrefab/Player").GetComponent<Inventory>();
        GC = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<GameController>();
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

    #region Buy Item
    public void BuyItem(int Number)
    {
        if (PM.Gold >= Value[Number] && inventory.SlotAvailable > 0)
        {
            PM.Gold -= Value[Number];
            inventory.AddItem(Item[Number]);
            FindItemsAmount();
        }
    }
    #endregion

    #region Sell Item
    public void SellItem(int Number)
    {
        if (inventory.CheckForItem(Item[Number]) == true)
        {
            inventory.RemoveItem(Item[Number]);
            PM.Gold += 200;
            FindItemsAmount();
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
    #endregion

    #region Buy Item Equipment
    public void BuyItem_Equipment(int Number)
    {
        if (PM.Gold >= Value[Number] && inventory.Equipment_SlotAvailable > 0)
        {
            PM.Gold -= Equipment_Value[Number];
            inventory.AddEquipmentItem(Equipment_Item[Number]);
        }
    }
    #endregion

    #region Sell Item Equipment
    public void SellItem_Equipment(int Number)
    {
        if (inventory.CheckForEquipmentItem(Equipment_Item[Number]) == true)
        {
            inventory.RemoveEquipmentItem(Equipment_Item[Number]);
            PM.Gold += 200;
        }
    }
    #endregion
}
