using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OutOfBounds : MonoBehaviour
{
    #region Variables
    public myPlayer player;
    public Animator canvas_Animator;
    public Transform newTransform;

    public UnityEvent Event;
    #endregion

    private void OnTriggerEnter2D(Collider2D Object)
    {
        if (Object.name == "Player")
        {
            player.Freeze();
            canvas_Animator.SetTrigger("FadeInOut");
            StartCoroutine(Delay());
        }
    }
    IEnumerator Delay()
    {
        Event.Invoke();
        yield return new WaitForSeconds(1f);
        player.gameObject.SetActive(false);
        player.transform.position = newTransform.position;
        player.transform.rotation = newTransform.rotation;
        player.gameObject.SetActive(true);
        player.LoseHP(2, false);
        yield return new WaitForSeconds(0.3f);
        player.Unfreeze();
    }
}
