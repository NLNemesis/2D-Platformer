using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Progress
{
    public float posX;
    public float posY;
    public Progress(SaveGameController SGC)
    {
        posX = SGC.player.transform.position.x;
        posY = SGC.player.transform.position.y;
    }
}
