using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class myPlacement : MonoBehaviour
{
    #region Variables
    private bool isClose;

    [Header("For Placement")]
    public string item;
    public string needText;
    public string useText;
    public bool placed;

    [Header("References")]
    private AudioSource thisSource;
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
        if (collision.name == "Player" && !placed)
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
        thisSource = GetComponent<AudioSource>();
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

            if (inventory.CheckForItem(item))
            {
                inventory.RemoveItem(item);
                playerInfoText.text = useText;
                canvas_Animator.SetTrigger("ShowInfo");
            }
            else
            {
                playerInfoText.text = needText;
                canvas_Animator.SetTrigger("ShowInfo");
            }
        }
    }

    #region Load Door
    public void LoadDoor()
    {
        thisSource.enabled = false;
        Event.Invoke();
    }
    #endregion
}
