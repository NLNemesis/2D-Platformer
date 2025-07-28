using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    #region Variables
    [Header("UI Controller")]
    public int UIOpened; //0 = clear, 1 = Pause Menu, 2 = Inventory....
    public GameObject Inventory;

    [Header("UI Variables")]
    public Slider HealthSlider;

    [Header("References")]
    public GameObject Character;
    private MyPlayerMovement MPM;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        MPM = Character.GetComponent<MyPlayerMovement>();
        HealthSlider.maxValue = MPM.Health;
        HealthSlider.value = MPM.Health;
    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = MPM.Health;

        if (Input.GetKeyDown(KeyCode.Tab) && UIOpened != 1)
            OpenAndCloseInventory();

        if (Input.GetKeyDown(KeyCode.Escape) && UIOpened == 2)
            OpenAndCloseInventory();
    }

    public void OpenAndCloseInventory()
    {
        if (!Inventory.activeSelf)
        {
            Inventory.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            UIOpened = 2;
        }
        else
        {
            Inventory.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            UIOpened = 0;
        }
    }

    public void ChangeLevel(int Number)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(Number);
    }
}
