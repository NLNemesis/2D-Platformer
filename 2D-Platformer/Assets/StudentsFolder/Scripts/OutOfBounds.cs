using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    #region Variables
    public myPlayer player;
    public Animator canvas_Animator;
    public Transform newTransform;
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
        yield return new WaitForSeconds(0.45f);
        player.gameObject.SetActive(false);
        player.transform.position = newTransform.position;
        player.transform.rotation = newTransform.rotation;
        player.gameObject.SetActive(true);
        player.LoseHP(2);
        yield return new WaitForSeconds(0.3f);
        player.Unfreeze();
    }
}
