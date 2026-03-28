using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class myDoor : MonoBehaviour
{
    #region Variables
    private bool isClose;
    public GameObject interaction_Indicator;

    [Space(10)]
    public bool Teleport;
    public Transform NewTransform;

    [Header("References")]
    public myPlayer player;
    public Animator canvas_Animator;

    [Header("Events")]
    public UnityEvent Event;
    #endregion

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isClose = true;
            interaction_Indicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isClose = false;
            interaction_Indicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isClose)
        {
            isClose = false;
            interaction_Indicator.SetActive(false);

            if (Teleport)
                StartCoroutine(SmoothTeleport());
            else
                Event.Invoke();
        }
    }

    IEnumerator SmoothTeleport()
    {
        isClose = false;
        interaction_Indicator.SetActive(false);
        player.Freeze();
        canvas_Animator.SetTrigger("FadeInOut");
        yield return new WaitForSeconds(1f);
        player.gameObject.SetActive(false);
        player.transform.position = NewTransform.position;
        player.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.45f);
        player.Unfreeze();
    }
}
