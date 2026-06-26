using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    #region Settings Menu
    [Header("References")]
    public SaveGameController SGC;
    [Header("Audio")]
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

    //Set UI
    public void Set_UI_Slider(float master, float sfx, float ambient)
    {
        masterSlider.value = master;
        sfxSlider.value = sfx;
        ambientSlider.value = ambient;
    }
    #endregion
}
