using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameController : MonoBehaviour
{
    public bool InMenu;
    public SettingsMenu sm;
    [Header("Progress")]
    public myPlayer player;
    public myInventory inventory;

    private void Awake()
    {
        if (InMenu)
        {
            Settings s = SaveSystem.LoadSettings();
            if (s != null)
                LoadSettings();
            else
                SaveSettings();
        }

        if (!InMenu)
        {
            //LoadSettings();
            LoadProgress();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SaveSystem.DeleteProgress();
            SaveSystem.SaveProgress(this);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadProgress();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveSystem.DeleteProgress();
        }
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
        sm.master = s.master;
        sm.mixer.SetFloat("master", s.master);
        sm.sfx = s.sfx;
        sm.mixer.SetFloat("sfx", s.sfx);
        sm.ambient = s.ambient;
        sm.mixer.SetFloat("ambient", s.ambient);
        sm.Set_UI_Slider(s.master, s.sfx, s.ambient);
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
            inventory.slotName = p.slotName;
            inventory.LoadInventory();
        }
        else
        {
            SaveSystem.SaveProgress(this);
        }
    }

    public void SaveProgress()
    {
        SaveSystem.SaveProgress(this);
    }
    #endregion
}
