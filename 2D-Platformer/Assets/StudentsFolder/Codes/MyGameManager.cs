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

    [Header("Pause Menu")]
    public UnityEvent OnPause;
    public UnityEvent OnResume;
    public GameObject[] DisableObject;
    private bool[] ObjectActivity;
    #endregion

    void Start()
    {
        ObjectActivity = new bool[DisableObject.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (UIOpened == 0 || UIOpened == 2)
            InventoryController();

        if (UIOpened == 0 || UIOpened == 1)
            PauseMenu();
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

    #region Pause Menu Controller
    public void PauseMenu()
    { 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIOpened == 0)
            {
                OnPause.Invoke();
                UIOpened = 1;
                for (int i = 0; i < DisableObject.Length; i++)
                {
                    ObjectActivity[i] = DisableObject[i].activeSelf;
                    DisableObject[i].SetActive(false);
                }
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            else
                Resume();
        }
    }

    public void Resume()
    {
        OnResume.Invoke();
        UIOpened = 0;
        for (int i = 0; i < DisableObject.Length; i++)
            DisableObject[i].SetActive(ObjectActivity[i]);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    #endregion

    public void ChangeLevel(int Number)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(Number);
    }
}
