using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyPickUpSystem : MonoBehaviour
{
    [Header("Controller")]
    private bool CanInteract;

    [Header("References & Objects")]
    public MyInventory inventory;
    public GameObject Message;

    [Header("Event")]
    public UnityEvent Interaction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Message.SetActive(true);
            CanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Message.SetActive(false);
            CanInteract = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanInteract)
        {
            Message.SetActive(false);
            CanInteract = false;
            Interaction.Invoke();
        }
    }
}
