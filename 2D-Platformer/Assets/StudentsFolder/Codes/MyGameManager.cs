using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    public int UIOpened;
    public GameObject PlayerInventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region Inventory Handler
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (PlayerInventory.activeSelf == false)
            {
                PlayerInventory.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                UIOpened = 2;
            }
            else
            {
                PlayerInventory.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                UIOpened = 0;
            }
        }
        #endregion
    
        #region Escape Button Handler
        if (Input.GetKeyDown(KeyCode.Escape) && UIOpened == 2)
        {
            PlayerInventory.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            UIOpened = 0;
        }
        #endregion
    }
}
