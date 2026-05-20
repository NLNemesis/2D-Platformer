using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject[] fungalDepthsMark;
    public GameObject[] forgottenDungeon;
    public GameObject[] darkRuins;
    public GameObject[] lookoutCliff;

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
            for (int i = 0; i < fungalDepthsMark.Length; i++)
                fungalDepthsMark[i].SetActive(false);
            fungalDepthsMark[id].SetActive(true);
            currentMark = id;
        }
        else if (currentLayout == 2)
        {
            for (int i = 0; i < forgottenDungeon.Length; i++)
                forgottenDungeon[i].SetActive(false);
            forgottenDungeon[id].SetActive(true);
            currentMark = id;
        }
        else if (currentLayout == 3)
        {
            for (int i = 0; i < darkRuins.Length; i++)
                darkRuins[i].SetActive(false);
            darkRuins[id].SetActive(true);
            currentMark = id;
        }
        else if (currentLayout == 4)
        {
            for (int i = 0; i < lookoutCliff.Length; i++)
                lookoutCliff[i].SetActive(false);
            lookoutCliff[id].SetActive(true);
            currentMark = id;
        }
    }
    #endregion

    #region Buttons
    public void Load_Retry()
    {
        StartCoroutine(OpenNewScene(1));
    }

    public void ToMainMenu()
    {
        StartCoroutine(OpenNewScene(0));
    }

    IEnumerator OpenNewScene(int sceneID)
    {
        Toggle_Cursor(false);
        canvasAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneID);
    }

    #endregion
}
