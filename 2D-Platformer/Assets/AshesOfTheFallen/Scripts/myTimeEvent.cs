using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class myTimeEvent : MonoBehaviour
{
    #region Variables
    public float Timer;
    public UnityEvent Event;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeEvent());
    }

    private void OnEnable()
    {
        StartCoroutine(TimeEvent());
    }

    public void CallTimeEvent()
    {
        StartCoroutine(TimeEvent());
    }

    IEnumerator TimeEvent()
    {
        yield return new WaitForSeconds(Timer);
        Event.Invoke();
    }
}
