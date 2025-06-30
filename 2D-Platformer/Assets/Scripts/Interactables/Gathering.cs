using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Gathering : MonoBehaviour
{
    #region Variables
    [Header("Tier")]
    public int Tier;
    public enum GatheringItemType {Wood,Ore,Hunting}
    public GatheringItemType ItemType;
    public float XPValue;
    public string ItemName;

    [Header("Messages")]
    private bool CanInteract;
    private GameObject[] Messages;

    [Header("References")]
    private PlayerMovement PM;
    private Inventory IC;
    private InputManager IM;
    private Animator CanvasAnimator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        IC = GameObject.Find("/MaxPrefab/Player").GetComponent<Inventory>();
        IM = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<InputManager>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
        Messages = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<GameController>().UIMessages;
    }

    #region On Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && ItemType == GatheringItemType.Wood)
        {
            Messages[9].SetActive(true);
            CanInteract = true;
        }
        else if (collision.tag == "Player" && ItemType == GatheringItemType.Ore)
        {
            Messages[10].SetActive(true);
            CanInteract = true;
        }
        else if (collision.tag == "Player" && ItemType == GatheringItemType.Hunting)
        {
            Messages[11].SetActive(true);
            CanInteract = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && ItemType == GatheringItemType.Wood)
        {
            Messages[9].SetActive(true);
            CanInteract = true;
        }
        else if (collision.tag == "Player" && ItemType == GatheringItemType.Ore)
        {
            Messages[10].SetActive(true);
            CanInteract = true;
        }
        else if (collision.tag == "Player" && ItemType == GatheringItemType.Hunting)
        {
            Messages[11].SetActive(true);
            CanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i = 0; i < Messages.Length; i++)
                Messages[i].SetActive(false);
            CanInteract = false;
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (CanInteract == true)
        {
            if (Input.GetKeyDown(IM.Interaction))
            {
                if (IC.SlotAvailable > 0)
                    GatherItem();
                else
                    CanvasAnimator.SetTrigger("InventoryFull");
            }
        }
    }

    #region Gather Item
    void GatherItem()
    {
        if (ItemType == GatheringItemType.Wood && PM.AxeTier >= Tier)
            AddItem();
        else if (ItemType == GatheringItemType.Ore && PM.PickaxeTier >= Tier)
            AddItem();
        else if (ItemType == GatheringItemType.Hunting && PM.KnifeTier >= Tier)
            AddItem();
        else
            CanvasAnimator.SetTrigger("HigherTier");
    }
    #endregion

    #region Add Item
    void AddItem()
    {
        StartCoroutine(PM.GainXP(XPValue));
        IC.AddItem(ItemName);
        for (int i = 0; i < Messages.Length; i++)
            Messages[i].SetActive(false);
        CanInteract = false;
        this.gameObject.SetActive(false);
    }
    #endregion
}
