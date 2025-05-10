using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    #region Variables
    public int UIOpened;
    public GameObject InventoryObject;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (UIOpened == 0 || UIOpened == 2)
            InventoryController();
    }

    void InventoryController()
    {
        #region Inventory handler
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!InventoryObject.activeSelf)
            {
                InventoryObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                UIOpened = 2;
            }
            else
            {
                InventoryObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                UIOpened = 0;
            }
        }
        #endregion

        #region Escape button Handler
        if (Input.GetKeyDown(KeyCode.Space) && UIOpened == 2)
        {
            InventoryObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            UIOpened = 0;
        }
        #endregion
    }
}
