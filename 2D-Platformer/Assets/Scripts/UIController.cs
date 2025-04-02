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
    [HideInInspector] public bool InUI;
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
    #endregion

    private void Start()
    {
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        AC = GameObject.Find("/MaxPrefab/Player").GetComponent<AnimController>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
        IM = GetComponent<InputManager>();
    }

    private void Update()
    {
        #region Inventory
        if (Input.GetKeyDown(IM.InventoryKey))
        {
            if (Inventory.activeSelf == false && InUI == false)
            {
                OpenCloseInventory();
            }
            else if (Inventory.activeSelf == true && InUI == true)
            {
                OpenCloseInventory();
            }
        }
        #endregion

        #region Assign Player Stats
        Stats[0].text = PM.Damage.ToString();
        Stats[1].text = PM.SkillDamage.ToString();
        Stats[2].text = PM.Armor.ToString();
        Stats[3].text = PM.MagicResist.ToString();
        Stats[4].text = PM.AxeTier.ToString();
        Stats[5].text = PM.PickaxeTier.ToString();
        Stats[6].text = PM.KnifeTier.ToString();
        #endregion
    }

    #region Open and Close Inventory
    public void OpenCloseInventory()
    {
        if (Inventory.activeSelf == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CanvasAnimator.SetTrigger("Inventory");
            InUI = true;
            AC.CanAttack = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CanvasAnimator.SetTrigger("Inventory");
            InUI = false;
            AC.CanAttack = false;
        }
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
