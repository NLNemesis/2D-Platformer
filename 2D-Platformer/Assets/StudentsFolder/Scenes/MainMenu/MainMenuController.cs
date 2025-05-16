using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public AudioMixer Master;

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ChangeAudioMaster(float Value)
    {
        Master.SetFloat("Master", Value);
    }
    public void ChangeAudioSFX(float Value)
    {
        Master.SetFloat("SFX", Value);
    }
    public void ChangeAudioAmbient(float Value)
    {
        Master.SetFloat("Ambient", Value);
    }
}
