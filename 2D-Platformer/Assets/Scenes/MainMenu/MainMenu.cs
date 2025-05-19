using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Variables
    [Header("Introduction")]
    public float IntroDelay;

    [Header("References")]
    public Animator animator; //MainMenuAnimator

    [Header("Events")]
    public UnityEvent IntroductionEvent;
    public UnityEvent StartEvent;
    public UnityEvent QuitEvent;

    [Header("Options")]
    public AudioMixer AudioManager;
    #endregion

    #region When the game loads
    void Start()
    {
        StartCoroutine(IntroductionDelay());
    }

    IEnumerator IntroductionDelay()
    {
        yield return new WaitForSeconds(IntroDelay);
        IntroductionEvent.Invoke();
    }
    #endregion

    void Update()
    {
        
    }

    public void ShowMainButtons() { animator.SetTrigger("ShowMainButtons"); }

    #region Start Button
    public void StartButton()
    {
        StartCoroutine(StartEventTrigger());
    }

    IEnumerator StartEventTrigger()
    {
        yield return new WaitForSeconds(1f);
        StartEvent.Invoke();
    }

    public void ToNewLevel() { SceneManager.LoadScene(1); }
    #endregion

    #region Change Audio (Options)
    public void ChangeMasterVolume(float Number)
    {
        AudioManager.SetFloat("Master", Number);
    }
    public void ChangeSFXVolume(float Number)
    {
        AudioManager.SetFloat("SFX", Number);
    }
    public void ChangeAmbientVolume(float Number)
    {
        AudioManager.SetFloat("Ambient", Number);
    }
    #endregion

    #region Quit Button
    public void QuitButton() 
    {
        StartCoroutine(QuitEventTrigger());
    }

    IEnumerator QuitEventTrigger()
    {
        yield return new WaitForSeconds(1f);
        QuitEvent.Invoke();
    }

    public void CloseTheApplication() { Application.Quit(); }
    #endregion

    public void OpenApplicationLink(string path)
    {
        Application.OpenURL(path);
    }
}
