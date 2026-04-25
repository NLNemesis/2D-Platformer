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

    #region Start
    private void Start()
    {
        StartCoroutine(VolumeUpRoutine());
    }

    IEnumerator VolumeUpRoutine()
    {
        mixer.SetFloat("master", -20);
        float Timer = 0f;
        float Duration = 15f;

        while (Timer < Duration)
        {
            Timer += Time.deltaTime; // ? actually advance time
            float step = Timer / Duration;
            float volumeUp = Mathf.Lerp(-20, 0, step); // ? lerp from fixed start, not current
            mixer.SetFloat("master", volumeUp);
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
        animator.Play("Buttons_Reverse");
        yield return new WaitForSeconds(2.3f);
        SceneManager.LoadScene(1);
    }
    #endregion

    #region Settings Menu
    public AudioMixer mixer;
    public float master;
    public Slider masterSlider;
    public float sfx;
    public Slider sfxSlider;
    public float ambient;
    public Slider ambientSlider;

    //Set Sliders and volume
    public void Set_Master_Volume(float volume)
    {
        master = volume;
        mixer.SetFloat("master", volume);
    }

    public void Set_Sfx_Volume(float volume)
    {
        sfx = volume;
        mixer.SetFloat("sfx", volume);
    }

    public void Set_Ambient_Volume(float volume)
    {
        ambient = volume;
        mixer.SetFloat("ambient", volume);
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
