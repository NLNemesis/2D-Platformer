using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [Header("Settings")]
    public bool DisableAfter;
    public bool CursorEnable;
    [Space(10)]
    public UnityEvent Event;

    private void OnTriggerEnter2D(Collider2D Object)
    {
        if (Object.name == "Player" || Object.name == "Character")
        {
            Event.Invoke();

            if (CursorEnable)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (DisableAfter)
                this.gameObject.SetActive(false);
        }
    }
}
