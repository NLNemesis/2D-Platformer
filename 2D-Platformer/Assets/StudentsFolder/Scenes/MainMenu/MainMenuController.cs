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
    #region Start
    private void Start()
    {
        StartCoroutine(VolumeUpRoutine());
    }

    IEnumerator VolumeUpRoutine()
    {
        Settings s = SaveSystem.LoadSettings();
        mixer.SetFloat("master", -20);
        float Timer = 0f;
        float Duration = 15f;

        while (Timer < Duration)
        {
            Timer += Time.deltaTime; // ? actually advance time
            float step = Timer / Duration;
            if (s != null)
            {
                float volumeUp = Mathf.Lerp(-20, s.master, step); // ? lerp from fixed start, not current
                mixer.SetFloat("master", volumeUp);
            }
            else
            {
                float volumeUp = Mathf.Lerp(-20, 0, step); // ? lerp from fixed start, not current
                mixer.SetFloat("master", volumeUp);
            }
            yield return null;
        }

        mixer.SetFloat("master", 0); // ensure it lands exactly at 0
    }
    #endregion

    #region Start Button
    public void StartButton()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        SGC.SaveSettings();
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
        SGC.SaveSettings();
        animator.Play("Buttons_Reverse");
        yield return new WaitForSeconds(2.3f);
        Application.Quit();
    }
    #endregion

}
