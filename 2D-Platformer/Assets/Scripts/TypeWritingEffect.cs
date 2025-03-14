using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TypeWritingEffect : MonoBehaviour
{
    public float delay = 0.1f;
    public float StartDelay = 0f;
    public string fulltext;
    private string currText = "";
    public bool loop = false;
    public UnityEvent OnTextShown;

    private bool TextShown;
    private TextMeshProUGUI testText;

    public void SetFullText(string newText) {
        fulltext = newText;
        if (wasEnabled) {
            OnEnable();
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {   
        testText = GetComponent<TextMeshProUGUI>();
        if (myRoutine != null) {
            StopCoroutine(myRoutine);
        }
        myRoutine = StartCoroutine(ShowText());
        wasEnabled = true;
    }

    bool wasEnabled = false;

    Coroutine myRoutine = null;
    bool openbrackets = false;

    bool UltraSpeed = false;

    public void SetUltraSpeed(bool toSet) {
        UltraSpeed = toSet;
    }

    IEnumerator ShowText()
    {   
        WaitForSeconds wfs = new WaitForSeconds(delay);
        WaitForSeconds half = new WaitForSeconds(delay/4f);

        if (StartDelay > 0f) {
            yield return new WaitForSeconds(StartDelay);
        }

        for (int i = 1; i <= fulltext.Length; i++)
        {
            currText = fulltext.Substring(0,i);
            testText.text = currText;

            if (UltraSpeed) {continue;}
            if (fulltext[i-1] == '<') {openbrackets = true;}
            else if (fulltext[i-1] == '>') {openbrackets = false;}
            else if (fulltext[i-1] == ' ') {continue;}
            else if (currText[i-1] == '-') { yield return half; continue;}
            else if (currText[i-1] == '.') { yield return new WaitForSeconds(1f);}

            if (openbrackets) {continue;}
            
            yield return wfs;
        }

        TextShown = true;

        if (loop) {
            testText.text = "";
            TextShown = false;
            StartAgain();
        }
    }

    public void ShowFullText()
    {
        StopAllCoroutines();
        testText.text = fulltext;
        TextShown = true;
    }

    void StartAgain() {
        if (myRoutine != null) {
            StopCoroutine(myRoutine);
        }
        myRoutine = StartCoroutine(ShowText());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && TextShown)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            OnTextShown.Invoke();
        }
    }

    void OnDisable() {
        StopAllCoroutines();
    }

}
