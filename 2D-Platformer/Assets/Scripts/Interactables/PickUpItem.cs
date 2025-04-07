using UnityEngine;
using UnityEngine.Events;

public class PickUpItem : MonoBehaviour
{
    #region Variables
    private bool CanInteract;
    public enum ItemType {Chest,Item}
    public ItemType Type;
    public float XPValue;
    public int Gold;

    [Header("Chest")]
    public float Tier;
    private bool Opened;
    public bool Locked;
    public string RequiredItem;

    [Header("Item")]
    public int GoldAmount;
    public string[] ItemName;

    [Header("Messages")]
    private GameObject[] Messages;

    [Header("References")]
    public UnityEvent Event;
    private Animator animator;
    private Inventory inventory;
    private PlayerMovement PM;
    private InputManager IM;
    private BoxCollider2D BC2D;
    private Animator CanvasAnimator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        IM = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<InputManager>();
        inventory = GameObject.Find("/MaxPrefab/Player").GetComponent<Inventory>();
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
        BC2D = GetComponent<BoxCollider2D>();
        Messages = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<UIController>().UIMessages;

        if (Type == ItemType.Chest) 
        { 
            animator = GetComponent<Animator>();
            animator.SetFloat("Chest", Tier);
        }
    }

    #region On Triggers
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Type == ItemType.Item)
        {
            Messages[0].SetActive(true);
            CanInteract = true;
        }
        else if (collision.tag == "Player" && Type == ItemType.Chest && Locked == true)
        {
            Messages[1].SetActive(true);
            CanInteract = true;
        }
        else if (collision.tag == "Player" && Type == ItemType.Chest && Locked == false)
        {
            Messages[2].SetActive(true);
            CanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i = 0; i < Messages.Length; i++)
            {
                Messages[i].SetActive(false);
            }
            CanInteract = false;
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (CanInteract == true)
        {
            if (Input.GetKeyDown(IM.Interaction) && Type == ItemType.Item)
            {
                if (inventory.SlotAvailable > ItemName.Length)
                    TakeItem();
                else
                    CanvasAnimator.SetTrigger("InventoryFull");
            }
            else if (Input.GetKeyDown(IM.Interaction) && Type == ItemType.Chest && Locked == true)
                UnlockChest();
            else if (Input.GetKeyDown(IM.Interaction) && Type == ItemType.Chest && Locked == false)
                if (inventory.SlotAvailable > ItemName.Length)
                    LootChest();
                else
                    CanvasAnimator.SetTrigger("InventoryFull");
        }
    }

    void TakeItem()
    {
        PM.GainXP(XPValue);
        for (int i = 0; i < ItemName.Length; i++) 
        {
            inventory.AddItem(ItemName[i]);
        }
        PM.UIText[2].text = "lots of loot";
        CanvasAnimator.SetTrigger("Took");
        Messages[0].SetActive(false);
        this.gameObject.SetActive(false);
    }

    void UnlockChest()
    {
        inventory.CheckForItem(RequiredItem);
        if(inventory.Check == true)
        {
            inventory.RemoveItem(RequiredItem);
            CanvasAnimator.SetTrigger("Used");
            PM.UIText[1].text = RequiredItem;
            Locked = false;
            Messages[1].SetActive(false);
        }
        else
        {
            CanvasAnimator.SetTrigger("Need");
            PM.UIText[0].text = RequiredItem;
        }
    }

    void LootChest()
    {
        PM.GainXP(XPValue);
        PM.Gold += Gold;
        animator.SetTrigger("Open");
        for (int i = 0; i < ItemName.Length; i++)
        {
            if (ItemName[i] == "Gold" || ItemName[i] == "Coins")
            {
                inventory.Gold += GoldAmount;
            }
            else
            {
                inventory.AddItem(ItemName[i]);
            }
        }
        PM.UIText[2].text = "lots of loot";
        if (animator != null)
        CanvasAnimator.SetTrigger("Took");
        Messages[2].SetActive(false);
        BC2D.enabled = false;
        Event.Invoke();
    }
}
