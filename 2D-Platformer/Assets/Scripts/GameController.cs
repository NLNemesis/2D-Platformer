using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;
using TMPro;
using UnityEngine.Events;
using System.Threading;

public class GameController : MonoBehaviour
{
    #region Variables
    public int OpenedUI;
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

    [Header("World")]
    public AudioClip[] WorldClip;
    private AudioSource WorldAS;
    #endregion

    private void Start()
    {
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        AC = GameObject.Find("/MaxPrefab/Player").GetComponent<AnimController>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
        IM = GetComponent<InputManager>();
        ObjectActivity = new bool[DisableObject.Length];
        StartEvent.Invoke();
        WorldAS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        AssignStats();
        if (PM.State == 1) return;

        if (Input.GetKeyDown(IM.InventoryKey) && (OpenedUI == 0 || OpenedUI == 2))
        {
            if (OpenedUI == 0)
                OpenCloseInventory();
            else if (OpenedUI == 2)
                EscapeHander();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            EscapeHander();
    }

    #region Escape Handler
    void EscapeHander()
    {
        if (OpenedUI == 2)
            OpenCloseInventory();
        else if (OpenedUI == 0 || OpenedUI == 1)
            PauseMenu();
    }
    #endregion

    #region Open and Close Inventory
    void OpenCloseInventory()
    {
        if (Inventory.activeSelf == false)
        {
            CursorOn();
            OpenedUI = 2;
            AC.CanAttack = false;
        }
        else
        {
            CursorOff();
            OpenedUI = 0;
            AC.CanAttack = true;
        }
        CanvasAnimator.SetTrigger("Inventory");
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
            OpenedUI = 1;
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
        OpenedUI = 0;
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

    #region Change World Ambient
    public void ChangeWorldAmbient(int Number)
    {
        StartCoroutine(ChangeVolumeLerp(Number));
    }

    IEnumerator ChangeVolumeLerp(int Number)
    {
        float Timer = 0;
        float Duration = 0.3f;
        float OriginalWorldASVolume = WorldAS.volume;
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            float Step = Timer / Duration;
            WorldAS.volume = Mathf.Lerp(WorldAS.volume, 0, Step);
            yield return null;
        }
        WorldAS.Stop();
        WorldAS.clip = WorldClip[Number];
        WorldAS.Play();
        Timer = 0;
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            float Step = Timer / Duration;
            WorldAS.volume = Mathf.Lerp(WorldAS.volume, OriginalWorldASVolume, Step);
            yield return null;
        }
    }
    #endregion

    #region Cursor Trigger Visibility
    public void CursorOn()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CursorOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion
}
