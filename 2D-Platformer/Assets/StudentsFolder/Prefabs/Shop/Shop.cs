using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shop : MonoBehaviour
{
    #region Variables
    private bool close;
    public GameObject message;

    public UnityEvent openShopEvent;
    public UnityEvent closeShopEvent;
    #endregion

    private void OnTriggerEnter2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            close = true;
            message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            close = false;
            message.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
