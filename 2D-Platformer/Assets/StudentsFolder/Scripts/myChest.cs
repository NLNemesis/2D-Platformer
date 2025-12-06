using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class myChest : MonoBehaviour
{
    private bool isClose;
    public GameObject Message;
    public UnityEvent OpenEvent;

    public string[] itemName;
    public Sprite[] itemIcon;
    public myInventory myinventory;

    private void OnTriggerEnter2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            isClose = true;
            Message.SetActive(true);
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isClose)
        {
            isClose = false;
            OpenEvent.Invoke();
            for (int i = 0; i < itemName.Length; i++)
                myinventory.AddItem(itemIcon[i], itemName[i]);
        }
    }
}
