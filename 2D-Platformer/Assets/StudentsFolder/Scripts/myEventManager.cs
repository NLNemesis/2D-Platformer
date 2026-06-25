using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class myEventManager : MonoBehaviour
{
    #region Variables
    [HideInInspector] public bool completed;
    [Header("Controller")]
    public bool multipleInteraction;
    public int interactionCounter;
    public UnityEvent interactionEvent;
    #endregion

    public void Call_Interaction()
    {
        if (multipleInteraction)
        {
            interactionCounter--;

            if (interactionCounter < 0)
            {
                interactionEvent.Invoke();
                completed = true;
            }
        }
    }

    public void Load_myEM()
    {
        interactionEvent.Invoke();
        completed = true;
    }
}
