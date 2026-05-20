using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveGameController : MonoBehaviour
{
    #region References
    public bool InMenu;
    public SettingsMenu sm;
    [Header("Progress")]
    public myPlayer player;
    public myInventory inventory;
    public myGameManager myGM;
    [Header("World")]
    public GameObject[] worldObject;
    public myEnemy[] enemy;
    public myChest[] chest;
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

    #region Update
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
        if (p != null)
        {
            //Load Player
            player.LoadHP(p.hp);
            player.gameObject.SetActive(false);
            Vector2 newPos = new Vector2(p.posX, p.posY);
            player.transform.position = newPos;
            player.gameObject.SetActive(true);
            inventory.slotName = p.slotName;
            inventory.LoadInventory();

            //Load Player UI
            myGM.ChangeMapLayout(p.currentLayout);
            myGM.ChangePointMark(p.currentMark);

            //Load World
            for (int i = 0; i < worldObject.Length; i++)
                worldObject[i].SetActive(p.activeObject[i]);

            for(int i = 0; i < chest.Length; i++)
            {
                chest[i].opened = p.opened[i];
                if (chest[i].opened)
                    chest[i].LoadChest();
            }

            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i].health = p.aiHealth[i];
                if (enemy[i].health <= 0)
                {
                    enemy[i].gameObject.SetActive(false);
                    Vector2 newAIPos = new Vector2(p.aiPosX[i], p.aiPosY[i]);
                    enemy[i].transform.position = newAIPos;
                    enemy[i].gameObject.SetActive(true);
                    enemy[i].TakeDamage(1);
                }
            }
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
