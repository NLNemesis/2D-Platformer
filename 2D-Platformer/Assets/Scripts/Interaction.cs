using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    #region Variables
    public enum InteractionType {Item,Chest,Door,Gather}
    public InteractionType Type;

    [Header("Interaction")]
    private bool CanInteract;
    public GameObject[] Message;

    [Header("Type = Item")]
    public string[] Item;

    [Header("Type = Chest,Door")]
    public bool Locked;
    public string Key;

    [Header("References")]
    private Inventory IC;
    private Animator animator;
    private Animator CanvasAnimator;
    private PlayerMovement PM;
    public UnityEvent UnlockEvent;
    #endregion

    void Start()
    {
        IC = GameObject.Find("/MaxPrefab/Player").GetComponent<Inventory>();
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();

        if (Type == InteractionType.Chest || Type == InteractionType.Door)
            animator = GetComponent<Animator>();
    }

    #region Interaction
    void OnTriggerStay2D(Collider2D Object)
    {
        if (Object.tag == "Player")
        {
            CanInteract = true;

            if (Type == InteractionType.Item)
                Message[0].SetActive(true);
            else if (Type == InteractionType.Gather)
                Message[1].SetActive(true);

            if (Locked == true)
                Message[0].SetActive(true);
            else if (Locked == false)
                Message[1].SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D Object)
    {
        if (Object.tag == "Player")
        {
            CanInteract = false;
            for (int i = 0; i < Message.Length; i++)
                Message[i].SetActive(false);
        }
    }
    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanInteract == true)
        {
            CanInteract = false;
            for (int i = 0; i < Message.Length; i++)
                Message[i].SetActive(false);

            if (Type == InteractionType.Item || Type == InteractionType.Gather)
            {
                AddItemsToThePlayer();
                this.gameObject.SetActive(false);
            }
            else
            {
                ChestOrDoor();
            }
        }
    }

    #region If Type = Chest or Door
    void ChestOrDoor()
    {
        if (Locked == false)
        {
            if (animator != null)
            animator.SetTrigger("Open");

            if (Type == InteractionType.Chest)
            AddItemsToThePlayer();

            UnlockEvent.Invoke();
        }
        else
        {
            IC.CheckForItem(Key);
            if (IC.Check == true)
            {
                IC.RemoveItem(Key);
                PM.UIText[1].text = Key;
                CanvasAnimator.SetTrigger("Used");
                Locked = false; 
            }
            else
            {
                PM.UIText[0].text = Key;
                CanvasAnimator.SetTrigger("Need");
            }
        }
    }
    #endregion

    #region Add the items to player's inventory
    void AddItemsToThePlayer()
    {
        for (int i = 0; i < Item.Length; i++)
        {
            IC.AddItem(Item[i]);
        }
        PM.UIText[2].text = "Lot's of loot";
        CanvasAnimator.SetTrigger("Took");
    }
    #endregion
}
