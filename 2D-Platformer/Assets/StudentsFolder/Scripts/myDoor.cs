using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class myDoor : MonoBehaviour
{
    #region Variables
    private bool isClose;

    [Header("Locked")]
    public bool locked;
    public string key;
    public string needText;
    public string useText;

    [Header("Telepoort")]
    public bool Teleport;
    public Transform NewTransform;

    [Header("References")]
    private myPlayer player;
    private myInventory inventory;
    private TextMeshProUGUI playerInfoText;
    private Animator canvas_Animator;
    private GameObject interaction_Indicator;

    [Header("Events")]
    public UnityEvent Event;
    #endregion

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            GrabReferences(collision);
            isClose = true;
            interaction_Indicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isClose = false;
            interaction_Indicator.SetActive(false);
        }
    }

    #region Grab References
    void GrabReferences(Collider2D Object)
    {
        player = Object.GetComponent<myPlayer>();
        inventory = Object.GetComponent<myInventory>();
        playerInfoText = Object.transform.root.GetComponentInChildren<myGameManager>().infoText;
        canvas_Animator = Object.transform.root.GetComponentInChildren<Canvas>().GetComponent<Animator>();
        interaction_Indicator = Object.transform.root.GetComponentInChildren<myGameManager>().interaction_Message;
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isClose)
        {
            isClose = false;
            interaction_Indicator.SetActive(false);

            if (locked)
            {
                bool hasItem = inventory.CheckForItem(key);
                if (hasItem)
                {
                    locked = false;
                    playerInfoText.text = useText;
                    canvas_Animator.SetTrigger("ShowInfo");
                    inventory.RemoveItem(key);
                    Event.Invoke();
                }
                else
                {
                    playerInfoText.text = needText;
                    canvas_Animator.SetTrigger("ShowInfo");
                }
            }
            else if (Teleport && locked)
            {
                bool hasItem = inventory.CheckForItem(key);
                if (hasItem)
                {
                    locked = false;
                    playerInfoText.text = useText;
                    canvas_Animator.SetTrigger("ShowInfo");
                    StartCoroutine(SmoothTeleport());
                }
                else
                {
                    playerInfoText.text = needText;
                    canvas_Animator.SetTrigger("ShowInfo");
                }
            }
            else if (Teleport && !locked)
                StartCoroutine(SmoothTeleport());
            else
                Event.Invoke();
        }
    }

    #region Smooth Teleport
    IEnumerator SmoothTeleport()
    {
        Event.Invoke();
        isClose = false;
        interaction_Indicator.SetActive(false);
        player.Freeze();
        canvas_Animator.SetTrigger("FadeInOut");
        yield return new WaitForSeconds(1f);
        player.gameObject.SetActive(false);
        player.transform.position = NewTransform.position;
        player.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.45f);
        player.Unfreeze();
    }
    #endregion
}
