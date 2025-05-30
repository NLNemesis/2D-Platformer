using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;
using TMPro;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    #region Variables
    [HideInInspector] public int OpenedUI;
    [Header("Object")]
    public GameObject Inventory;

    [Header("Reference")]
    private PlayerMovement PM;
    private AnimController AC;
    private Animator CanvasAnimator;
    private InputManager IM;

    public GameObject[] UIMessages;

    [Header("Player Stats UI")]
    public TextMeshProUGUI[] Stats;
    
    [Header("Extras")]
    public TextMeshProUGUI NewAreaText;

    public UnityEvent StartEvent;

    [Header("Pause Menu")]
    public UnityEvent OnPause;
    public UnityEvent OnResume;
    public GameObject[] DisableObject;
    private bool[] ObjectActivity;
    #endregion

    private void Start()
    {
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        AC = GameObject.Find("/MaxPrefab/Player").GetComponent<AnimController>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
        IM = GetComponent<InputManager>();
        ObjectActivity = new bool[DisableObject.Length];
        StartEvent.Invoke();
    }

    private void Update()
    {
        if (PM.State == 1) return;

        if (Input.GetKeyDown(IM.InventoryKey) && OpenedUI == 0)
            OpenCloseInventory();
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(IM.InventoryKey)) && OpenedUI == 2)
            OpenCloseInventory();

        if (Input.GetKeyDown(KeyCode.Escape) && (OpenedUI == 0 || OpenedUI == 1))
            PauseMenu();

        AssignStats();
    }

    #region Open and Close Inventory
    void OpenCloseInventory()
    {
        if (Inventory.activeSelf == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CanvasAnimator.SetTrigger("Inventory");
            OpenedUI = 2;
            AC.CanAttack = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CanvasAnimator.SetTrigger("Inventory");
            OpenedUI = 0;
            AC.CanAttack = false;
        }
    }
    #endregion

    #region Pause Menu
    void PauseMenu()
    {
        if (OpenedUI == 0)
        {
            OnPause.Invoke();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            for (int i = 0; i < DisableObject.Length; i++)
            {
                ObjectActivity[i] = DisableObject[i].activeSelf;
                DisableObject[i].SetActive(false);
            }
            Time.timeScale = 0;
        }
        else 
            Resume();
    }

    public void Resume()
    {
        OnResume.Invoke();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        for (int i = 0; i < DisableObject.Length; i++)
            DisableObject[i].SetActive(ObjectActivity[i]);
        Time.timeScale = 1;
    }

    public void ChangeLevel(int Number)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(Number);
    }
    #endregion

    #region Assign Player Stats
    void AssignStats()
    {
        Stats[0].text = PM.Damage.ToString();
        Stats[1].text = PM.SkillDamage.ToString();
        Stats[2].text = PM.Armor.ToString();
        Stats[3].text = PM.MagicResist.ToString();
        Stats[4].text = PM.AxeTier.ToString();
        Stats[5].text = PM.PickaxeTier.ToString();
        Stats[6].text = PM.KnifeTier.ToString();
    }
    #endregion

    #region Open Message
    public void ShowMessage(int Number)
    {
        for (int i = 0; i < UIMessages.Length; i++) 
        { 
            if (Number == i)
            {
                UIMessages[i].SetActive(true);
            }
            else
            {
                UIMessages[i].SetActive(false);
            }
        }
    }
    #endregion
}
