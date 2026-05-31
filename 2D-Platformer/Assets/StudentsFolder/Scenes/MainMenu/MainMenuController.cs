using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Animator animator;
    public SaveGameController SGC;
    public AudioMixer mixer;

    public GameObject Load_Delete_File;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    #region Start Button
    public void StartButton(int difficulty)
    {
        StartCoroutine(StartGame(difficulty));
    }

    IEnumerator StartGame(int difficulty)
    {
        SGC.difficulty = difficulty;
        SGC.SaveSettings();
        animator.Play("Buttons_Reverse");
        yield return new WaitForSeconds(2.3f);
        SceneManager.LoadScene(1);
    }
    #endregion

    #region Load Button
    public void FirstLoadButton()
    {
        Progress p = SaveSystem.LoadProgress();

        if (p != null)
            Load_Delete_File.SetActive(true);
        else
            StartCoroutine(LoadGame());
    }

    public void LoadButton()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        animator.Play("Buttons_Reverse");
        yield return new WaitForSeconds(2.3f);
        SceneManager.LoadScene(1);
    }

    public void DeleteLoad()
    {
        SaveSystem.DeleteProgress();
    }
    #endregion

    #region Quit Button
    public void QuitButton()
    {
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        SGC.SaveSettings();
        animator.Play("Buttons_Reverse");
        yield return new WaitForSeconds(2.3f);
        Application.Quit();
    }
    #endregion
}
