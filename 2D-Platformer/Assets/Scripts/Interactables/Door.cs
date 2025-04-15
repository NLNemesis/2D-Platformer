using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    #region Variables
    private bool CanInteract;
    public enum DoorType {Classic, Travel}
    public DoorType Type;
    private bool Traveling;

    [Header("Door")]
    public bool Locked;
    private bool Opened;
    public string RequiredItem;

    [Header("Travel")]
    public Transform NewPlace;

    [Header("Messages")]
    private GameObject[] Messages;

    [Header("References")]
    public UnityEvent UnlockDoorEvent;
    public UnityEvent OpenDoorEvent;
    private Animator animator;
    private GameObject Player;
    private Inventory IC;
    private PlayerMovement PM;
    private InputManager IM;
    private Animator CanvasAnimator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("/MaxPrefab/Player");
        IC = Player.GetComponent<Inventory>();
        PM = Player.GetComponent<PlayerMovement>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
        IM = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<InputManager>();
        Messages = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<UIController>().UIMessages;
    }

    #region On Triggers
    private void OnTriggerStay2D(Collider2D collision)
    {
        #region Show Messages
        if (Traveling) return;

        if (collision.tag == "Player" && Locked == true)
        {
            Messages[4].SetActive(true);
            CanInteract = true;
        }
        else if (collision.tag == "Player" && Type == DoorType.Classic)
        {
            if (Opened == false)
            {
                Messages[5].SetActive(true);
                CanInteract = true;
            }
            else
            {
                Messages[6].SetActive(true);
                CanInteract = true;
            }
        }
        else if (collision.tag == "Player" && Type == DoorType.Travel)
        {
            Messages[7].SetActive(true);
            CanInteract = true;
        }
        #endregion
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i = 0; i < Messages.Length; i++)
            {
                Messages[i].SetActive(false);
            }
            CanInteract = false;
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (CanInteract == true)
        {
            if (Input.GetKeyDown(IM.Interaction) && Locked == true)
            {
                UnlockDoor();
            }
            else if (Input.GetKeyDown(IM.Interaction) && Type == DoorType.Classic)
            {
                CanInteract = false;
                ClassicDoor();
            }
            else if (Input.GetKeyDown(IM.Interaction) && Locked == false && Type == DoorType.Travel)
            {
                CanInteract = false;
                Messages[7].SetActive(false);
                StartCoroutine(TraveToNewPlace());
            }
        }
    }

    #region Unlock the door
    void UnlockDoor()
    {
        IC.CheckForItem(RequiredItem);
        if (IC.Check == true)
        {
            IC.RemoveItem(RequiredItem);
            Locked = false;
            CanvasAnimator.SetTrigger("Used");
            PM.UIText[1].text = RequiredItem;
            Messages[4].SetActive(true);
            UnlockDoorEvent.Invoke();
        }
        else
        {
            CanvasAnimator.SetTrigger("Need");
            PM.UIText[0].text = RequiredItem;
        }
    }
    #endregion

    #region Open and close the door
    public void ClassicDoor()
    {
        Opened = true;
        Messages[5].SetActive(false);

        animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Open");

        OpenDoorEvent.Invoke();
    }
    #endregion

    #region Travel the player
    IEnumerator TraveToNewPlace()
    {
        Traveling = true;
        PM.Freezed = true;
        CanvasAnimator.SetTrigger("FadeInOut");
        yield return new WaitForSeconds(1f);
        Player.transform.position = NewPlace.position;
        yield return new WaitForSeconds(1f);
        PM.Freezed = false;
        Traveling = false;
    }
    #endregion
}
