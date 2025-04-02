using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FastTravel : MonoBehaviour
{
    #region Variables
    public bool CanSelect;
    [Header("If the can select is false")]
    public int NewScene;

    [Header("Reference")]
    private PlayerMovement PM;
    private Animator CanvasAnimator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        CanvasAnimator = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Animator>();
    }

    #region OnTriggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && CanSelect)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CanvasAnimator.SetTrigger("FastTravel");
            PM.PlayerFreeze = true;
            PM.AC.animator.SetFloat("State", 0);
        }
        else
        {
            StartCoroutine(LoadingProcess(NewScene));
        }
    }
    #endregion

    public void Cancel()
    {
        PM.PlayerFreeze = false;
        CanvasAnimator.SetTrigger("ResetTravel");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SelectNewScene(int SceneToLoad)
    {
        StartCoroutine(LoadingProcess(SceneToLoad));
    }

    IEnumerator LoadingProcess(int SceneToLoad)
    {
        CanvasAnimator.SetTrigger("ResetTravel");
        CanvasAnimator.SetTrigger("FadeIn");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(1f);
        AsyncOperation Operation = SceneManager.LoadSceneAsync(SceneToLoad);
        Operation.allowSceneActivation = false;
        float Progress = 0;
        while (!Operation.isDone)
        {
            Progress = Mathf.MoveTowards(Progress, Operation.progress, Time.deltaTime);

            if (Progress >= 0.9f)
            {
                Progress = 1;
                Operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
