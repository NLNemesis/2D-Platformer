using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Talk : MonoBehaviour
{
    #region Variables
    [Header("Variables")]
    public Sprite NPCIcon;
    private bool CanInteract;
    private bool Talking;
    private GameObject[] Messages;

    [Header("References")]
    private PlayerMovement PM;
    private Inventory IC;
    private QuestSystem QS;
    private InputManager IM;
    private AIMove aiMove;

    [Header("UI")]
    public GameObject ChattingCanvas;
    public Image NPCFrame;
    public Image PlayerFrame;
    public GameObject[] NPCLine;
    public GameObject PlayerLine;

    [Header("Assign Quest")]
    public bool MainQuest;
    public string Quest;
    public GameObject[] QuestText;
    private bool GaveQuest;
    private bool QuestCompleted;
    public string[] RequiredItems;

    [Header("Quest Rewards")]
    public int XP;
    public int Gold;
    public string[] Reward;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        aiMove = GetComponent<AIMove>();
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        IC = GameObject.Find("/MaxPrefab/Player").GetComponent<Inventory>();
        QS = GameObject.Find("/MaxPrefab/Player").GetComponent<QuestSystem>();
        IM = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<InputManager>();
        Messages = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<UIController>().UIMessages;
        NPCFrame.sprite = NPCIcon;
        PlayerFrame.sprite = PM.PlayerIcon;
    }

    #region On Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PM.InInteaction == false)
        {
            CanInteract = true;
            Messages[8].SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Talking == false && PM.InInteaction == false)
        {
            CanInteract = true;
            Messages[8].SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanInteract = false;
            Messages[8].SetActive(false);
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(IM.Interaction) && CanInteract == true)
        {
            ResetChatting();
            OpenChat();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Talking == true)
        {
            CloseChat();
            ResetChatting();
        }
    }

    #region Open and Close Chat
    public void OpenChat()
    {
        CanInteract = false;
        Messages[8].SetActive(false);
        ChattingCanvas.SetActive(true);
        Talking = true;
        PM.PlayerFreeze = true;
        PM.InInteaction = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (aiMove.Speed > 0)
        {
            aiMove.animator.SetFloat("Movement", 0);
            aiMove.AIFreeze = true;
        }
    }

    public void CloseChat()
    {
        Talking = false;
        PM.PlayerFreeze = false;
        PM.InInteaction = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (aiMove.Speed > 0) 
        {
            aiMove.animator.SetFloat("Movement", 1);
            aiMove.AIFreeze = false;
        }
        ChattingCanvas.SetActive(false);
    }
    #endregion

    #region Chat with NPC
    public void Chatting(int Number)
    {
        PlayerFrame.color = Color.grey;
        NPCFrame.color = Color.white;

        PlayerLine.SetActive(false);

        NPCLine[Number].SetActive(true);
    }

    public void ResetChatting()
    {
        PlayerFrame.color = Color.white;
        NPCFrame.color = Color.grey;

        PlayerLine.SetActive(true);

        for (int i = 0; i < NPCLine.Length; i++) 
        {
            NPCLine[i].SetActive(false);
        }

        for (int i = 0; i < QuestText.Length; i++)
        {
            QuestText[i].SetActive(false);
        }
    }
    #endregion

    #region NPC Quest System
    public void AssignQuest()
    {
        if (GaveQuest == false)
        {
            GaveQuest = true;
            PlayerFrame.color = Color.grey;
            NPCFrame.color = Color.white;

            PlayerLine.SetActive(false);

            QuestText[0].SetActive(true);

            if (MainQuest == true)
            {
                QS.AssignMainQuest(Quest);
            }
            else
            {
                //QS.AssignSideQuest(QuestText);
            }
        }
        else if (QuestCompleted == false)
        {
            GaveQuest = true;
            PlayerFrame.color = Color.grey;
            NPCFrame.color = Color.white;

            PlayerLine.SetActive(false);

            QuestText[1].SetActive(true);
        }
        else
        {
            GaveQuest = true;
            PlayerFrame.color = Color.grey;
            NPCFrame.color = Color.white;

            PlayerLine.SetActive(false);

            QuestText[4].SetActive(true);
        }
    }

    public void CompleteQuest()
    {
        if (QuestCompleted == false)
        {
            bool Check = true;
            for (int i = 0; i < RequiredItems.Length; i++)
            {
                IC.CheckForItem(RequiredItems[i]);

                if (IC.Check == false)
                {
                    Check = false;
                    break;
                }
            }

            if (Check == false)
            {
                PlayerFrame.color = Color.grey;
                NPCFrame.color = Color.white;
                PlayerLine.SetActive(false);
                QuestText[2].SetActive(true);
            }
            else
            {
                PlayerFrame.color = Color.grey;
                NPCFrame.color = Color.white;
                PlayerLine.SetActive(false);
                QuestText[3].SetActive(true);
                QuestCompleted = true;
                for (int i = 0; i < RequiredItems.Length; i++)
                {
                    IC.RemoveItem(RequiredItems[i]);
                }
                StartCoroutine(PM.GainXP(XP));
                PM.Gold += Gold;
                for (int i = 0; i < Reward.Length; i++)
                {
                    IC.AddItem(Reward[i]);
                }
            }
        }
        else
        {
            GaveQuest = true;
            PlayerFrame.color = Color.grey;
            NPCFrame.color = Color.white;

            PlayerLine.SetActive(false);

            QuestText[4].SetActive(true);
        }
    }
    #endregion
}
