using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public AudioMixer AudioMaster;

    #region Main Buttons    
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
    #endregion

    #region Audio Buttons
    public void ChangeMaster(float Value)
    {
        AudioMaster.SetFloat("Master", Value);
    }
    public void ChangeSFX(float Value)
    {
        AudioMaster.SetFloat("SFX", Value);
    }
    public void ChangeAmbient(float Value)
    {
        AudioMaster.SetFloat("Ambient", Value);
    }
    #endregion
}
