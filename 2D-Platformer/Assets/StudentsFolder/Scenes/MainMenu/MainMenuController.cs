using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Animator animator;

    #region Start Button
    public void StartButton()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        animator.Play("Buttons_Reverse");
        yield return new WaitForSeconds(2.3f);
        SceneManager.LoadScene(1);
    }
    #endregion

    #region Quit Button
    public void QuitButton()
    {
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        animator.Play("Buttons_Reverse");
        yield return new WaitForSeconds(2.3f);
        Application.Quit();
    }
    #endregion

}
