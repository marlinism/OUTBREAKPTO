using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TopSubspriteManager : SubspriteManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Run base.PlayAnimation() and change sortingOrder based on animation direction
    public override void PlayAnimation(string action, string direction, bool flipX)
    {
        base.PlayAnimation(action, direction, flipX);

        if (direction.Contains("back"))
        {
            sr.sortingOrder = 1;
        }
        else
        {
            sr.sortingOrder = -1;
        }
    }

    public override void UpdateDirection(string direction, bool flipX)
    {
        base.UpdateDirection(direction, flipX);

        if (direction.Contains("back"))
        {
            sr.sortingOrder = 1;
        }
        else
        {
            sr.sortingOrder = -1;
        }
    }
}
