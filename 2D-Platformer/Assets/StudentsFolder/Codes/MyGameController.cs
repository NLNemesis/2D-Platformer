using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MyGameController : MonoBehaviour
{
    #region Variables
    public int OpenedUI;

    [Header("Pause Menu")]
    public UnityEvent OnPause;
    public UnityEvent OnResume;
    public GameObject[] CheckObject;
    private bool[] DisabledObject;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        DisabledObject = new bool[CheckObject.Length];
    }

    // Update is called once per frame
    void Update()
    {
        CheckPauseMenu();
    }

    #region Pause Menu
    public void CheckPauseMenu()
    {
        if (!Input.GetKey(KeyCode.Escape)) return;

        if (OpenedUI == 0)
        {
            OnPause.Invoke();
            for (int i = 0; i < CheckObject.Length; i++)
            {
                DisabledObject[i] = CheckObject[i].activeSelf;
                CheckObject[i].SetActive(false);
            }
            OpenedUI = 1;
            Time.timeScale = 0;
        }
        else if (OpenedUI == 1)
        {
            OnResume.Invoke();
            for (int i = 0; i < CheckObject.Length; i++)
                CheckObject[i].SetActive(DisabledObject[i]);
            OpenedUI = 0;
            Time.timeScale = 1;
        }
    }
    #endregion
    public void ChangeScene(int Number)
    {
        SceneManager.LoadScene(Number);
    }

    public void ChangeOpenedUIInt(int Number)
    {
        OpenedUI = Number;
    }
}
