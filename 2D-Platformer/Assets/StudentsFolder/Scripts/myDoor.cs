using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class myDoor : MonoBehaviour
{
    #region Variables
    private bool isClose;
    public GameObject interaction_Indicator;

    [Header("Locked")]
    public bool locked;
    public string key;
    public string needText;
    public string useText;

    [Header("Telepoort")]
    public bool Teleport;
    public Transform NewTransform;

    [Header("References")]
    public myPlayer player;
    public myInventory inventory;
    public TextMeshProUGUI playerInfoText;
    public Animator canvas_Animator;

    [Header("Events")]
    public UnityEvent Event;
    #endregion

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
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
