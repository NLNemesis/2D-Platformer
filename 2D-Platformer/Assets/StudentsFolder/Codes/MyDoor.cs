using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyDoor : MonoBehaviour
{
    #region Variables
    [Header("Locked System")]
    public bool Locked;
    public string Key;

    bool Opened;
    bool CanInteract;

    [Header("Reference")]
    public GameObject[] Message;
    public Inventory inventory;
    public UnityEvent OpenedEvent;
    #endregion

    #region On triggers
    void OnTriggerStay2D(Collider2D Object)
    {
        if (Object.name == "Player" && Opened == false)
        {
            CanInteract = true;
            if (Locked == true)
            {
                Message[0].SetActive(true);
                Message[1].SetActive(false);
            }
            else
            {
                Message[0].SetActive(false);
                Message[0].SetActive(true);
            }
        }
    }

    void OnTrggerExit2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            CanInteract = false;
            Message[0].SetActive(false);
            Message[1].SetActive(false);
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanInteract == true)
        {
            if (Locked == true)
            {
                inventory.CheckForItem(Key);
                if (inventory.CheckForItem(Key))
                {
                    inventory.RemoveItem(Key);
                    Locked = false;
                }
            }
            else
            {
                Opened = true;
                CanInteract = false;
                Message[0].SetActive(false);
                Message[1].SetActive(false);
                OpenedEvent.Invoke();
            }
        }
    }
}
