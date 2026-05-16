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
    public bool[] opened;

    //Enemy
    public int[] aiHealth;
    public float[] aiPosX;
    public float[] aiPosY;
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

        opened = new bool[SGC.chest.Length];
        for (int i = 0; i < SGC.chest.Length; i++)
            opened[i] = SGC.chest[i].opened;

        aiHealth = new int[SGC.enemy.Length];
        aiPosX = new float[SGC.enemy.Length];
        aiPosY = new float[SGC.enemy.Length];
        for (int i = 0; i <  SGC.enemy.Length; i++)
        {
            aiHealth[i] = SGC.enemy[i].health;
            aiPosX[i] = SGC.enemy[i].transform.position.x;
            aiPosY[i] = SGC.enemy[i].transform.position.y;
        }
    }
}
