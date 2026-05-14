using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class myGameManager : MonoBehaviour
{
    public int UI;
    public GameObject inventory;


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

    #region Guide the player
    [Header("Guide the player")]
    public Animator canvasAnimator;
    public TextMeshProUGUI infoText;

    public void GuideThePlayer(string text)
    {
        infoText.text = text;
        canvasAnimator.SetTrigger("Guide");
    }
    #endregion

    #region Map Layout and Point Marks
    [Header("Map")]
    public int currentLayout;
    public GameObject[] Layout;
    public void ChangeMapLayout(int id)
    {
        for (int i = 0; i < Layout.Length; i++)
            Layout[i].SetActive(false);
        Layout[id].SetActive(true);
        currentLayout = id;
    }

    public int currentMark;
    public GameObject[] PointMark;
    public void ChangePointMark(int id)
    {
        for (int i = 0; i < PointMark.Length; i++)
            PointMark[i].SetActive(false);
        PointMark[id].SetActive(true);
        currentMark = id;
    }
    #endregion
}
