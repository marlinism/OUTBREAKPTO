using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BottomSubspriteManager : SubspriteManager
{
    // Offset components to change parent local position
    public int xPixelOffset = 0;
    public int yPixelOffset = 0;
    private const float PIXEL_DISTANCE = 1f / 32f; // 32 pixels per unit

    // Move Offsetted group transform object
    private Transform offsetGroup;

    // Original offsetGroup local position
    private Vector3 originalPosition;

    // Component References
    private Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        Assert.IsNotNull(animator);

        offsetGroup = transform.parent.transform.parent;

        originalPosition = offsetGroup.transform.localPosition;
    }

    // Offset parent local position by xPixelOffset and yPixelOffset
    void OnAnimatorMove()
    {
        offsetGroup.localPosition = new Vector3(
            originalPosition.x + xPixelOffset * PIXEL_DISTANCE, 
            originalPosition.y + yPixelOffset * PIXEL_DISTANCE, 
            originalPosition.z);
    }
}
