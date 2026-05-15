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
    public GameObject[] cavernMark;
    public GameObject[] FungalDepthsMark;
    public void ChangePointMark(int id)
    {
        if (currentLayout == 0)
        {
            for (int i = 0; i < cavernMark.Length; i++)
                cavernMark[i].SetActive(false);
            cavernMark[id].SetActive(true);
            currentMark = id;
        }
        else if (currentLayout == 1)
        {
            for (int i = 0; i < FungalDepthsMark.Length; i++)
                FungalDepthsMark[i].SetActive(false);
            FungalDepthsMark[id].SetActive(true);
            currentMark = id;
        }
    }
    #endregion
}
