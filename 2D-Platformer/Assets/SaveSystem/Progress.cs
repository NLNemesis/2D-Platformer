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
    public int soulEssence;
    public int coins;

    //Player UI
    public int currentLayout;
    public int currentMark;

    //World
    public int ambientClipID;
    public bool[] activeObject;
    public bool[] activePlatform;
    public bool[] openedDoor;
    public bool[] myEventManager;
    public bool[] myPlacement;
    //Chests
    public bool[] opened;
    public bool[] isLocked;

    //Enemy
    public int[] aiHealth;
    public Progress(SaveGameController SGC)
    {
        //Save Player
        hp = SGC.player.HP;
        posX = SGC.player.transform.position.x;
        posY = SGC.player.transform.position.y;
        slotName = SGC.inventory.slotName;
        soulEssence = SGC.inventory.soulEssence;
        coins = SGC.inventory.coins;

        //Save Player UI
        currentLayout = SGC.myGM.currentLayout;
        currentMark = SGC.myGM.currentMark;

        //Save World
        ambientClipID = SGC.myGM.ambientClipID;

        activeObject = new bool[SGC.worldObject.Length];
        for (int i = 0; i < SGC.worldObject.Length; i++)
            activeObject[i] = SGC.worldObject[i].activeSelf;

        openedDoor = new bool[SGC.door.Length];
        for (int i = 0; i < SGC.door.Length; i++)
            openedDoor[i] = SGC.door[i].opened;

        activePlatform = new bool[SGC.platform.Length];
        for (int i = 0; i < SGC.platform.Length; i++)
            activePlatform[i] = SGC.platform[i].canMove;

        myEventManager = new bool[SGC.myEventManager.Length];
        for (int i = 0; i < SGC.myEventManager.Length; i++)
            myEventManager[i] = SGC.myEventManager[i].completed;

        myPlacement = new bool[SGC.myPlacement.Length];
        for (int i = 0; i < SGC.myPlacement.Length; i++)
            myPlacement[i] = SGC.myPlacement[i].placed;

        //Save Chests
        opened = new bool[SGC.chest.Length];
        for (int i = 0; i < SGC.chest.Length; i++)
            opened[i] = SGC.chest[i].opened;

        isLocked = new bool[SGC.chest.Length];
        for (int i = 0; i < SGC.chest.Length; i++)
            isLocked[i] = SGC.chest[i].locked;

        //Save AI
        aiHealth = new int[SGC.enemy.Length];
        for (int i = 0; i <  SGC.enemy.Length; i++)
            aiHealth[i] = SGC.enemy[i].health;
    }
}
