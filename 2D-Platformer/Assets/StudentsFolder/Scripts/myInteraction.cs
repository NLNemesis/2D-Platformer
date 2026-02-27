using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class myInteraction : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
    public KeyCode interaction_Key;
    public bool Locked;

    private bool isClose;
    [Header("References")]
    public GameObject indicator;

    [Header("Events")]
    public UnityEvent interaction_Event;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isClose = true;
            indicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isClose = false;
            indicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(interaction_Key) && isClose)
        {
            isClose = false;
            interaction_Event.Invoke();
        }
    }
}
