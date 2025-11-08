using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour
{
    private bool isClose;
    public GameObject Message;
    public UnityEvent OpenEvent;

    private void OnTriggerEnter2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            isClose = true;
            Message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            isClose = false;
            Message.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isClose)
        {
            isClose = false;
            OpenEvent.Invoke();
        }
    }
}
