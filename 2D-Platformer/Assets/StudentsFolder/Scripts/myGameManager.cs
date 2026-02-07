using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myGameManager : MonoBehaviour
{
    public int UI;
    public GameObject inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region Toggle Inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inventory.activeSelf)
            {
                inventory.SetActive(true);
                UI = 1;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                inventory.SetActive(false);
                UI = 0;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && UI == 1)
        {
            inventory.SetActive(false);
            UI = 0;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion
    }

    #region Toggle Cursor
    public void Toggle_Cursor(bool state)
    {
        if (state)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    #endregion
}
