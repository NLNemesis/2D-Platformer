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

    //Player UI
    public int currentLayout;
    public int currentMark;

    //World
    public bool[] activeObject;
    public bool[] activePlatform;
    //Chests
    public bool[] opened;
    public bool[] isLocked;

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

        //Save Player UI
        currentLayout = SGC.myGM.currentLayout;
        currentMark = SGC.myGM.currentMark;

        //Save World
        activeObject = new bool[SGC.worldObject.Length];
        for (int i = 0; i < SGC.worldObject.Length; i++)
            activeObject[i] = SGC.worldObject[i].activeSelf;

        activePlatform = new bool[SGC.platform.Length];
        for (int i = 0; i < SGC.platform.Length; i++)
            activePlatform[i] = SGC.platform[i].canMove;

        //Save Chests
        opened = new bool[SGC.chest.Length];
        for (int i = 0; i < SGC.chest.Length; i++)
            opened[i] = SGC.chest[i].opened;
        isLocked = new bool[SGC.chest.Length];
        for (int i = 0; i < SGC.chest.Length; i++)
            isLocked[i] = SGC.chest[i].locked;

        //Save AI
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
