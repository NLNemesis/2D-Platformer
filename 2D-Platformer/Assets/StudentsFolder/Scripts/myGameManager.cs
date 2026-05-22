using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class myGameManager : MonoBehaviour
{
    public int UI;
    [Header("References")]
    public GameObject inventory;
    public Animator canvasAnimator;
    public TextMeshProUGUI infoText;


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

    #region Conversation
    [Header("Conversation")]
    public TextMeshProUGUI[] characterText;

    private Queue<string> textQueue = new Queue<string>();
    private Queue<int> characterQueue = new Queue<int>();
    private bool isGuiding = false;

    public void SylvaneTalk(string text)
    {
        textQueue.Enqueue(text);
        characterQueue.Enqueue(0);
        if (!isGuiding)
            StartCoroutine(ProcessGuideQueue());
    }

    public void ArthurTalk(string text)
    {
        textQueue.Enqueue(text);
        characterQueue.Enqueue(1);
        if (!isGuiding)
            StartCoroutine(ProcessGuideQueue());
    }

    private IEnumerator ProcessGuideQueue()
    {
        isGuiding = true;

        while (textQueue.Count > 0)
        {
            string text = textQueue.Dequeue();
            int id = characterQueue.Dequeue();
            characterText[id].text = text;
            canvasAnimator.SetTrigger("CharacterTalk"+id);

            // Wait for the animation to finish
            yield return null; // wait one frame so animator updates
            yield return new WaitForSeconds(5f);
        }

        isGuiding = false;
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
