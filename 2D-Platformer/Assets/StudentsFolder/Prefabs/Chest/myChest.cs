using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class myChest : MonoBehaviour
{
    #region Variables
    [Header("Controls")]
    public GameObject Message;
    public UnityEvent OpenEvent;
    [HideInInspector] public bool opened;
    private bool isClose;
    [Header("Locked")]
    public bool locked;
    public string key;
    public string lockedMessage;

    [Header("Container")]
    public string[] itemName;
    public Sprite[] itemIcon;
    private myInventory inventory;

    [Header("References")]
    private myGameManager gm;
    #endregion

    #region On Triggers
    private void OnTriggerEnter2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            isClose = true;
            Message.SetActive(true);
            inventory = Object.GetComponent<myInventory>();
            gm = Object.transform.root.GetComponentInChildren<myGameManager>();
        }
    }

    private void OnTriggerExit2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            isClose = false;
            Message.SetActive(false);
        }
    }
    #endregion

    #region Start / Update
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isClose)
        {
            if (locked)
            {
                if (inventory.CheckForItem(key))
                {
                    locked = false;
                    inventory.RemoveItem(key);
                    isClose = false;
                    OpenEvent.Invoke();
                    for (int i = 0; i < itemName.Length; i++)
                        inventory.AddItem(itemIcon[i], itemName[i]);
                    opened = true;
                }
                else
                {
                    gm.infoText.text = lockedMessage;
                    gm.canvasAnimator.SetTrigger("ShowInfo");
                }
            }
            else
            {
                isClose = false;
                OpenEvent.Invoke();
                for (int i = 0; i < itemName.Length; i++)
                    inventory.AddItem(itemIcon[i], itemName[i]);
                opened = true;
            }
        }
    }
    #endregion

    #region Load Chest
    public void LoadChest(bool isLocked)
    {
        AudioSource thisAudio = GetComponent<AudioSource>();
        thisAudio.volume = 0;
        locked = isLocked;
        isClose = false;
        OpenEvent.Invoke();
        opened = true;
    }
    #endregion
}
