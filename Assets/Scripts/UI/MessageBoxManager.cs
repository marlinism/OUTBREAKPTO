using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MessageBoxManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI messageText;

    [SerializeField]
    private float transitionTime = 0.25f;

    [SerializeField]
    private float messageBoxWidth = 1000f;

    private RectTransform messageBoxTransform;

    private bool shown = false;
    private bool showCoroutineStarted = false;
    private bool hideCoroutineStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(messageText);

        messageBoxTransform = GetComponent<RectTransform>();
        Assert.IsNotNull(messageBoxTransform);
    }

    void OnDisable()
    {
        Vector2 size = messageBoxTransform.sizeDelta;
        size.x = 0f;
        messageBoxTransform.sizeDelta = size;

        shown = false;
        showCoroutineStarted = false;
        hideCoroutineStarted = false;
    }

    public void Show(string text)
    {
        if (!shown && !showCoroutineStarted)
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowCoroutine(text));
        }
    }

    public void Hide()
    {
        if (shown && !hideCoroutineStarted)
        {
            StartCoroutine(HideCoroutine());
        }
    }

    private IEnumerator ShowCoroutine(string text)
    {
        while (hideCoroutineStarted)
        {
            yield return null;
        }
        showCoroutineStarted = true;
        messageText.text = text;

        float startTime = Time.time;
        Vector2 currSize = messageBoxTransform.sizeDelta;
        while (Time.time - startTime < transitionTime)
        {
            currSize.x = Mathf.Lerp(0, messageBoxWidth, (Time.time - startTime) / transitionTime);
            messageBoxTransform.sizeDelta = currSize;
            yield return null;
        }

        currSize = messageBoxTransform.sizeDelta;
        currSize.x = messageBoxWidth;
        messageBoxTransform.sizeDelta = currSize;

        showCoroutineStarted = false;
        shown = true;
        yield break;
    }

    private IEnumerator HideCoroutine()
    {
        while (showCoroutineStarted)
        {
            yield return null;
        }
        hideCoroutineStarted = true;

        float startTime = Time.time;
        Vector2 currSize = messageBoxTransform.sizeDelta;
        while (Time.time - startTime < transitionTime)
        {
            currSize.x = Mathf.Lerp(messageBoxWidth, 0, (Time.time - startTime) / transitionTime);
            messageBoxTransform.sizeDelta = currSize;
            yield return null;
        }

        currSize = messageBoxTransform.sizeDelta;
        currSize.x = 0f;
        messageBoxTransform.sizeDelta = currSize;

        hideCoroutineStarted = false;
        shown = false;
        gameObject.SetActive(false);
        yield break;
    }
}
