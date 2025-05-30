using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Merchant_Classic : MonoBehaviour
{
    #region Variables
    [Header("Interaction")]
    public GameObject Message;
    public GameObject ThisCanvas;
    public GameObject PlayerHUB;
    private bool CanInteract;

    [Header("Shop")]
    public TextMeshProUGUI GoldText;
    public string[] ItemName;
    public int[] Value;
    public int SellValue;

    [Header("References")]
    private PlayerMovement PM;
    private Inventory inventory;
    private Animator PlayerCanvasAnimator;

    [Header("Events")]
    public UnityEvent OpenShopEvent;
    public UnityEvent CloseShopEvent;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        inventory = GameObject.Find("/MaxPrefab/Player").GetComponent<Inventory>();
        PlayerCanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
    }

    #region OnTriggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !ThisCanvas.activeSelf)
        {
            CanInteract = true;
            Message.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && !ThisCanvas.activeSelf)
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanInteract)
            OpenCloseShop();

        if (Input.GetKeyDown(KeyCode.Escape) && ThisCanvas.activeSelf)
            OpenCloseShop();

        GoldText.text = PM.Gold.ToString();
    }

    #region Open and Close Shop
    public void OpenCloseShop()
    {
        if (ThisCanvas.activeSelf == false)
        {
            PlayerHUB.SetActive(false);
            ThisCanvas.SetActive(true);
            PM.Freezed = true;
            //UIM.HaveAccess = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            OpenShopEvent.Invoke();
        }
        else
        {
            PlayerHUB.SetActive(true);
            ThisCanvas.SetActive(false);
            PM.Freezed = false;
            //UIM.HaveAccess = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CloseShopEvent.Invoke();
        }
    }
    #endregion

    #region Buy Item
    public void BuyItem(int Number)
    {
        if (PM.Gold >= Value[Number])
        {
            if (inventory.SlotAvailable > 0)
            {
                PM.Gold -= Value[Number];
                inventory.AddItem(ItemName[Number]);
            }
            else
            {
                PlayerCanvasAnimator.SetTrigger("Full");
            }
        }
    }
    #endregion

    #region Sell Item
    public void SellItem(int Number)
    {
        inventory.CheckForItem(ItemName[Number]);
        if (inventory.Check)
        {
            inventory.RemoveItem(ItemName[Number]);
            PM.Gold += SellValue;
        }
    }
    #endregion
}

#region Extra Code
/* 
 //private UIManager UIM;
 //private WeaponManager WM;

 //WM = GameObject.Find("/MaxPrefab/Player").GetComponent<WeaponManager>();
 //UIM = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<UIManager>();
 
 if (Type[Number] == ItemType.Stackable)
 {
    PM.Gold -= Value[Number];
    IC.AddSI(ItemName[Number]);
 }
 else if (Type[Number] == ItemType.Weapon && WM.SlotAvailable > 0)
 {
    PM.Gold -= Value[Number];
    WM.AddWeapon(ItemName[Number]);
 }
 else if (Type[Number] == ItemType.Shield && WM.S_SlotAvailable > 0)
 {
    PM.Gold -= Value[Number];
    WM.AddShield(ItemName[Number]);
 }
 */
#endregion
