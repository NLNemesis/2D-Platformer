using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyItem : MonoBehaviour
{
    #region Variables
    private bool InRange;
    private bool Opened;

    [Header("References")]
    private Inventory inventory;
    private Animator Chest;
    private BoxCollider2D BC2D;

    [Header("Interaction")]
    public GameObject Message;
    public string[] Item;
    #endregion

    #region On Triggers
    private void OnTriggerStay2D(Collider2D Object)
    {
        if (Object.name == "Player" && Opened == false && inventory.SlotAvailable > 0)
        {
            InRange = true;
            Message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            InRange = false;
            Message.SetActive(false);
        }
    }
    #endregion

    #region Start and Update
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("/MaxPrefab/Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && InRange == true)
        {
            InRange = false;
            Message.SetActive(false);
            for (int i = 0; i < Item.Length; i++)
            {
                inventory.AddItem(Item[i]);
            }
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}
