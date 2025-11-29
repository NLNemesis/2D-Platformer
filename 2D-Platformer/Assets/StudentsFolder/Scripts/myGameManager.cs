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
        if (Input.GetKeyDown(KeyCode.Tab) && !inventory.activeSelf)
        {
            inventory.SetActive(true);
            UI = 1;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && inventory.activeSelf)
        {
            inventory.SetActive(false);
            UI = 0;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
