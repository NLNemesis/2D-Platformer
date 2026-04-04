using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour
{
    public AudioMixer mixer;

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
