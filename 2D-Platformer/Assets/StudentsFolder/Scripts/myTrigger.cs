using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class myTrigger : MonoBehaviour
{
    private bool isClose;
    [Header("Control")]
    public bool needsInput;
    [Header("References")]
    public GameObject Button;
    [Header("Event")]
    public UnityEvent Event;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !needsInput)
            Event.Invoke();

        if (collision.name == "Player" && needsInput)
        {
            isClose = true;
            Button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" && needsInput)
        {
            isClose = false;
            Button.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isClose && needsInput)
        {
            Button.SetActive(false);
            isClose = false;
            Event.Invoke();
        }
    }
}
