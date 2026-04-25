using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public float master;
    public float sfx;
    public float ambient;
    public Settings(SaveGameController SGC)
    {
        master = SGC.mmc.master;
        sfx = SGC.mmc.sfx;
        ambient = SGC.mmc.ambient;
    }
}
