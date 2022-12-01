using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Assertions;

public class InteractMarkerManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform markerTransform;

    [SerializeField]
    private TMPro.TextMeshProUGUI markerText;

    [SerializeField]
    private float transitionTime = 0.1f;

    [SerializeField]
    private float markerHeight = 56.25f;

    private bool shown = false;
    private bool showCoroutineStarted = false;
    private bool hideCoroutineStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(markerTransform);
        Assert.IsNotNull(markerText);

        InputUser.onChange += ControlsChanged;
    }

    void OnDisable()
    {
        Vector2 size = markerTransform.sizeDelta;
        size.y = 0f;
        markerTransform.sizeDelta = size;
        shown = false;
    }

    public void Show()
    {
        if (!shown && !showCoroutineStarted)
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowCoroutine());
        }
    }

    public void Hide()
    {
        if (shown && !hideCoroutineStarted)
        {
            StartCoroutine(HideCoroutine());
        }
    }

    private IEnumerator ShowCoroutine()
    {
        while (hideCoroutineStarted)
        {
            yield return null;
        }
        showCoroutineStarted = true;

        float startTime = Time.time;
        Vector2 currSize = markerTransform.sizeDelta;
        while (Time.time - startTime < transitionTime)
        {
            currSize.y = Mathf.Lerp(0, markerHeight, (Time.time - startTime) / transitionTime);
            markerTransform.sizeDelta = currSize;
            yield return null;
        }

        currSize = markerTransform.sizeDelta;
        currSize.y = markerHeight;
        markerTransform.sizeDelta = currSize;

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
        Vector2 currSize = markerTransform.sizeDelta;
        while (Time.time - startTime < transitionTime)
        {
            currSize.y = Mathf.Lerp(markerHeight, 0, (Time.time - startTime) / transitionTime);
            markerTransform.sizeDelta = currSize;
            yield return null;
        }

        currSize = markerTransform.sizeDelta;
        currSize.y = 0;
        markerTransform.sizeDelta = currSize;

        hideCoroutineStarted = false;
        shown = false;
        gameObject.SetActive(false);
        yield break;
    }

    private void ControlsChanged(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change != InputUserChange.ControlSchemeChanged)
        {
            return;
        }

        if (user.controlScheme.Value.name == "Gamepad")
        {
            markerText.text = "A";
        }
        else
        {
            markerText.text = "E";

        }
    }
}
