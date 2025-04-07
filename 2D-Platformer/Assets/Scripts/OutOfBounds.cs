using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    #region Variables
    [Header("Variables")]
    public Transform NewLocation;

    [Header("References")]
    private PlayerMovement PM;
    #endregion

    void Start()
    {
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PM.Freezed = true;
            PM.Health -= 50;
            PM.gameObject.transform.position = NewLocation.position;
            PM.gameObject.transform.rotation = NewLocation.rotation;
            PM.Freezed = false;
        }
    }
}
