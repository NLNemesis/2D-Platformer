using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TypeWritingEffect : MonoBehaviour
{
    public bool fade;
    public bool fadeinout;

    [Header("If Fade")]
    public float Duration;
    [Header("If FadeInOut")]
    public float TimeForFade;
    public float DurationOut;

    [Header("Type writting effect")]
    public float delay = 0.1f;
    public float StartDelay = 0f;
    public string fulltext;
    private string currText = "";
    public bool loop = false;

    [Header("Events")]
    public UnityEvent StartTyping;
    public UnityEvent OnTextShown;

    private bool TextShown;
    private TextMeshProUGUI testText;

    // Start is called before the first frame update
    void OnEnable()
    {
        TextShown = false;
        StartTyping.Invoke();
        testText = GetComponent<TextMeshProUGUI>();
        if (myRoutine != null)
        {
            StopCoroutine(myRoutine);
        }

        if (fade)
            StartCoroutine(FadeText());
        else if (fadeinout)
            StartCoroutine(FadeInOutText());
        else
        {
            myRoutine = StartCoroutine(ShowText());
            wasEnabled = true;
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && TextShown == true)
        {
            OnTextShown.Invoke();
        }
    }

    #region Type writting
    public void SetFullText(string newText)
    {
        fulltext = newText;
        if (wasEnabled)
        {
            OnEnable();
        }
    }

    bool wasEnabled = false;

    Coroutine myRoutine = null;
    bool openbrackets = false;

    bool UltraSpeed = false;

    public void SetUltraSpeed(bool toSet)
    {
        UltraSpeed = toSet;
    }

    IEnumerator ShowText()
    {
        WaitForSeconds wfs = new WaitForSeconds(delay);
        WaitForSeconds half = new WaitForSeconds(delay / 4f);

        if (StartDelay > 0f)
        {
            yield return new WaitForSeconds(StartDelay);
        }

        for (int i = 1; i <= fulltext.Length; i++)
        {
            currText = fulltext.Substring(0, i);
            testText.text = currText;

            if (UltraSpeed) { continue; }
            if (fulltext[i - 1] == '<') { openbrackets = true; }
            else if (fulltext[i - 1] == '>') { openbrackets = false; }
            else if (fulltext[i - 1] == ' ') { continue; }
            else if (currText[i - 1] == '-') { yield return half; continue; }
            if (openbrackets) { continue; }

            yield return wfs;
        }

        TextShown = true;

        if (loop)
        {
            testText.text = "";
            TextShown = false;
            StartAgain();
        }
    }

    void StartAgain()
    {
        if (myRoutine != null)
        {
            StopCoroutine(myRoutine);
        }
        myRoutine = StartCoroutine(ShowText());
    }

    public void ShowFullText()
    {
        testText.text = fulltext;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
    #endregion

    #region Text Fade Out-->In
    IEnumerator FadeText()
    {
        testText.color = new Color(255, 255, 255, 0);
        Color FullColor = new Color(255, 255, 255, 255);
        float Timer = 0;
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            float Step = Timer / Duration;
            testText.color = Color.Lerp(testText.color, FullColor, Step);
            yield return null;
        }
        TextShown = true;
        OnTextShown.Invoke();
    }
    #endregion

    #region Text Fade Out-->In-->Out
    IEnumerator FadeInOutText()
    {
        testText.color = new Color(255, 255, 255, 0);
        Color FullColor = new Color(255, 255, 255, 255);
        float Timer = 0;
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            float Step = Timer / Duration;
            testText.color = Color.Lerp(testText.color, FullColor, Step);
            yield return null;
        }
        TextShown = true;
        OnTextShown.Invoke();
        yield return new WaitForSeconds(TimeForFade);
        Timer = 0;
        FullColor = new Color(255, 255, 255, 0);
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            float Step = Timer / Duration;
            testText.color = Color.Lerp(testText.color, FullColor, Step);
            yield return null;
        }
    }
    #endregion
}
