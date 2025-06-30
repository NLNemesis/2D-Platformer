using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Exploration : MonoBehaviour
{
    #region Variables
    [Header("Variables")]
    public string AreaName;
    public UnityEvent Event;

    [Header("References")]
    private GameController GC;
    private Animator CanvasAnimator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.Find("/MaxPrefab/GameScripts").GetComponent<GameController>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GC.NewAreaText.text = AreaName;
            CanvasAnimator.SetTrigger("NewArea");
            Event.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
