using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameController : MonoBehaviour
{
    public bool InMenu;
    public MainMenuController mmc;
    [Header("Progress")]
    public myPlayer player;

    private void Awake()
    {
        Settings s = SaveSystem.LoadSettings();
        if (s != null)
        {
            LoadSettings();
        }
        else
        {
            SaveSettings();
        }

        if (!InMenu)
            LoadProgress();
    }

    #region Save/Load Settings
    public void SaveSettings()
    {
        SaveSystem.DeleteSettings();
        SaveSystem.SaveSettings(this);
    }

    void LoadSettings()
    {
        Settings s = SaveSystem.LoadSettings();
        #region Assign Saved Audio
        mmc.master = s.master;
        mmc.masterSlider.value = s.master;
        mmc.mixer.SetFloat("master", s.master);
        mmc.sfx = s.sfx;
        mmc.sfxSlider.value = s.sfx;
        mmc.mixer.SetFloat("sfx", s.sfx);
        mmc.ambient = s.ambient;
        mmc.ambientSlider.value = s.ambient;
        mmc.mixer.SetFloat("ambient", s.ambient);
        #endregion
    }
    #endregion

    #region Save/Load Progress
    public void LoadProgress()
    {
        Progress p = SaveSystem.LoadProgress();
        if (p != null)
        {
            player.gameObject.SetActive(false);
            Vector2 newPos = new Vector2(p.posX, p.posY);
            player.transform.position = newPos;
            player.gameObject.SetActive(true);
        }
        else
        {
            SaveSystem.SaveProgress(this);
        }
    }
    #endregion
}
