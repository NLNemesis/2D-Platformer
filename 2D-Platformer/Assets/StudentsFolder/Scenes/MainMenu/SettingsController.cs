using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour
{
    public AudioMixer mixer;

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

    public void Set_Master_Volume(float volume)
    {
        mixer.SetFloat("master", volume);
    }

    public void Set_Sfx_Volume(float volume)
    {
        mixer.SetFloat("sfx", volume);
    }

    public void Set_Ambient_Volume(float volume)
    {
        mixer.SetFloat("ambient", volume);
    }
}
