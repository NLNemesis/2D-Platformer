using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Progress
{
    //Player
    public int hp;
    public float posX;
    public float posY;
    public string[] slotName;

    //World
    public bool[] activeObject;
    public Progress(SaveGameController SGC)
    {
        //Save Player
        hp = SGC.player.HP;
        posX = SGC.player.transform.position.x;
        posY = SGC.player.transform.position.y;
        slotName = SGC.inventory.slotName;

        //Save World
        activeObject = new bool[SGC.worldObject.Length];
        for (int i = 0; i < SGC.worldObject.Length; i++)
            activeObject[i] = SGC.worldObject[i].activeSelf;
    }
}
