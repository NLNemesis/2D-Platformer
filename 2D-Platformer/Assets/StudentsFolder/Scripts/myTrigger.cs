using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class myTrigger : MonoBehaviour
{
    public UnityEvent Event;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
            Event.Invoke();
    }
}
