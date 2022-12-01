using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthBarManager : MonoBehaviour
{
    // Health bar restraints and information
    [SerializeField]
    private float minWidth = 300f;
    [SerializeField]
    private float maxWidth = 900f;
    [SerializeField]
    private float emptyBarOffset = 34f;

    // Component references
    [SerializeField]
    private List<RectTransform> scalableElements;
    [SerializeField]
    private RectTransform fillBar;

    private float prevFillPercent;

    // Properties
    public float MinimumWidth
    {
        get { return minWidth; }
    }
    public float MaximumWidth
    {
        get { return maxWidth; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsTrue(scalableElements.Count > 0);
        Assert.IsNotNull(fillBar);

        prevFillPercent = 1f;
    }

    // Set the width of the health bar
    public void SetWidth(float width)
    {
        if (width < minWidth)
        {
            width = minWidth;
        }
        else if (width > maxWidth)
        {
            width = maxWidth;
        }

        foreach(RectTransform element in scalableElements)
        {
            Vector2 dimensions = element.sizeDelta;
            dimensions.x = width;
            element.sizeDelta = dimensions;
        }

        SetFillPosition(prevFillPercent);
    }

    // Increase the width of the health bar
    public void IncreaseWidth(float widthIncrease)
    {
        Vector2 dimensions = fillBar.sizeDelta;
        float estimate = dimensions.x + widthIncrease;
        if (estimate > maxWidth)
        {
            SetWidth(maxWidth);
            return;
        }

        widthIncrease *= 1 - (estimate / maxWidth);
        SetWidth(widthIncrease + dimensions.x);
    }

    // Set the health bar fill position
    // fillPercent is a float ranging from 0 to 1
    // 0 represents an empty health bar
    // 1 represents a full health bar
    public void SetFillPosition(float fillPercent)
    {
        if (fillPercent < 0)
        {
            fillPercent = 0;
        }
        else if (fillPercent > 1)
        {
            fillPercent = 1;
        }

        Vector3 pos = fillBar.localPosition;
        pos.x = Mathf.Lerp(-(fillBar.sizeDelta.x - emptyBarOffset), 0, fillPercent);
        fillBar.localPosition = pos;
        prevFillPercent = fillPercent;
    }
}
