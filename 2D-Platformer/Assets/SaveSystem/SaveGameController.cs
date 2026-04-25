using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameController : MonoBehaviour
{
    public bool InMenu;
    public MainMenuController mmc;

    private void Awake()
    {
        Settings s = SaveSystem.LoadSettings();
        if (s != null)
        {
            mmc.master = s.master;
            mmc.sfx = s.sfx;
            mmc.ambient = s.ambient;
            mmc.Set_Master_Volume(s.master);
            mmc.Set_Sfx_Volume(s.sfx);
            mmc.Set_Ambient_Volume(s.ambient);
        }
        else
        {
            Debug.Log("Save settings not found");
            SaveSystem.SaveSettings(this);
        }
    }
}
