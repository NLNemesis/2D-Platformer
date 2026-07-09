using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SaveGameController : MonoBehaviour
{
    #region References
    public bool InMenu;
    [Header("Settings")]
    public int difficulty;
    public SettingsMenu sm;
    [Header("Progress")]
    public myPlayer player;
    public myInventory inventory;
    public myGameManager myGM;
    [Header("World")]
    public GameObject[] worldObject;
    public myDoor[] door;
    public myEnemy[] enemy;
    public myChest[] chest;
    public myPlatform[] platform;
    public myEventManager[] myEventManager;
    public myPlacement[] myPlacement;
    [Header("Events")]
    public UnityEvent notLoadFileEvent;
    public UnityEvent LoadFileEvent;
    #endregion

    #region Awake
    private void Start()
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
            myGM.Toggle_Cursor(false);
            LoadSettings();
            LoadProgress();
        }
    }
    #endregion

    #region Save/Load Settings
    public void SaveSettings()
    {
        SaveSystem.DeleteSettings();
        SaveSystem.SaveSettings(this);
    }

    void LoadSettings()
    {
        Settings s = SaveSystem.LoadSettings();
        if (s != null)
        {
            difficulty = s.difficulty;
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
        else SaveSystem.SaveSettings(this);
    }
    #endregion

    #region Save/Load Progress
    public void LoadProgress()
    {
        Progress p = SaveSystem.LoadProgress();
        Settings s = SaveSystem.LoadSettings();
        if (p != null)
        {
            try
            {
                LoadFileEvent.Invoke();
                //Load Player
                player.LoadHP(p.hp);
                player.gameObject.SetActive(false);
                Vector2 newPos = new Vector2(p.posX, p.posY);
                player.transform.position = newPos;
                player.gameObject.SetActive(true);
                inventory.slotName = p.slotName;
                inventory.LoadInventory();
                inventory.soulEssence = p.soulEssence;
                inventory.coins = p.coins;

                //Load Player UI
                myGM.ChangeMapLayout(p.currentLayout);
                myGM.ChangePointMark(p.currentMark);

                //Load World
                myGM.PlayAmbient(p.ambientClipID);

                for (int i = 0; i < worldObject.Length; i++)
                    worldObject[i].SetActive(p.activeObject[i]);

                for (int i = 0; i < door.Length; i++)
                    if (p.openedDoor[i])
                        door[i].LoadDoor();

                for (int i = 0; i < platform.Length; i++)
                    platform[i].canMove = p.activePlatform[i];

                for (int i = 0; i < myEventManager.Length; i++)
                    if (p.myEventManager[i])
                        myEventManager[i].Load_myEM();

                for (int i = 0; i < myPlacement.Length; i++)
                    if (p.myPlacement[i])
                        myPlacement[i].LoadPlacement();

                //Load Chests
                for (int i = 0; i < chest.Length; i++)
                {
                    chest[i].opened = p.opened[i];
                    if (chest[i].opened)
                        chest[i].LoadChest(p.isLocked[i]);
                }

                //Load Enemies
                for (int i = 0; i < enemy.Length; i++)
                {
                    enemy[i].health = p.aiHealth[i];
                    if (enemy[i].health <= 0)
                        enemy[i].LoadDead();
                }
            }
            catch
            {
                SaveSystem.DeleteProgress();
                SaveSystem.SaveProgress(this);
                AssignDifficulty(s.difficulty);
                notLoadFileEvent.Invoke();
            }
            AssignDifficulty(s.difficulty);
        }
        else
        {
            SaveSystem.SaveProgress(this);
            AssignDifficulty(s.difficulty);
            notLoadFileEvent.Invoke();
        }
    }

    #region Assign Difficulty
    void AssignDifficulty(int id)
    {
        Debug.Log("Assign Difficulty");
        myEnemy[] enemy = FindObjectsOfType<myEnemy>();
        myEnemyDetect[] myED = FindObjectsOfType<myEnemyDetect>();
        List <myEnemyDetect> classicEnemy = new List <myEnemyDetect>();
        List<myEnemyDetect> bossEnemy = new List<myEnemyDetect>();

        for (int i = 0; i < myED.Length; i++)
        {
            if (enemy != null && enemy[i].category == "Classic")
                classicEnemy.Add(myED[i]);
            else if (enemy != null && enemy[i].category == "Boss")
                bossEnemy.Add(myED[i]);
        }

        //Assign Health
        for (int i = 0; i < enemy.Length; i++)
        {
            if (id == 0 && enemy[i].health > 0)
                if (enemy[i].category == "Classic")
                    enemy[i].health = 5;
                else if (enemy[i].category == "Boss")
                    enemy[i].health = 20;
            else if (id == 1 && enemy[i].health > 0)
                    if (enemy[i].category == "Classic")
                        enemy[i].health = 15;
                    else if (enemy[i].category == "Boss")
                        enemy[i].health = 40;
            if (id == 2 && enemy[i].health > 0)
                if (enemy[i].category == "Classic")
                    enemy[i].health = 15;
                else if (enemy[i].category == "Boss")
                    enemy[i].health = 55;
            if (id == 3 && enemy[i].health > 0)
                if (enemy[i].category == "Classic")
                    enemy[i].health = 20;
                else if (enemy[i].category == "Boss")
                    enemy[i].health = 70;
            enemy[i].healthBar.maxValue = enemy[i].health;
            enemy[i].healthBar.value = enemy[i].health;
        }

        //Assign Damage
        for (int i = 0; i < classicEnemy.Count; i++)
        {
            if (id == 0 && enemy[i].health > 0)
                classicEnemy[i].damage = 1;
            else if (id == 1 && enemy[i].health > 0)
                classicEnemy[i].damage = 3;
            else if (id == 2 && enemy[i].health > 0)
                classicEnemy[i].damage = 6;
            else if (id == 3 && enemy[i].health > 0)
                classicEnemy[i].damage = 50;
        }

        //Assain Boss Damage
        for (int i = 0; i < bossEnemy.Count; i++)
        {
            if (id == 0 && enemy[i].health > 0)
                bossEnemy[i].damage = 3;
            else if (id == 1 && enemy[i].health > 0)
                bossEnemy[i].damage = 5;
            else if (id == 2 && enemy[i].health > 0)
                bossEnemy[i].damage = 7;
            else if (id == 3 && enemy[i].health > 0)
                bossEnemy[i].damage = 50;
        }
    }
    #endregion

    public void SaveProgress()
    {
        SaveSystem.SaveProgress(this);
    }
    #endregion
}
