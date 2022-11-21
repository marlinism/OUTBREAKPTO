using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum EnemyOrientation
{
    FrontRight,
    BackRight,
    FrontLeft,
    BackLeft
}

public class EnemySpriteManager : MonoBehaviour
{

    [SerializeField]
    private string actorName;
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Animator anim;

    // Properties
    public EnemyOrientation Orientation
    {
        get; set;
    }

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(actorName);
        Assert.IsNotNull(sr);
        Assert.IsNotNull(anim);
    }

    public void PlayAnimation(string actionName)
    {
        string animName = actorName + "_" + actionName;

        switch(Orientation)
        {
            case EnemyOrientation.FrontRight:
                anim.Play(animName + "_front");
                sr.flipX = false;
                return;

            case EnemyOrientation.BackRight:
                anim.Play(animName + "_back");
                sr.flipX = false;
                return;

            case EnemyOrientation.FrontLeft:
                anim.Play(animName + "_front");
                sr.flipX = true;
                return;

            case EnemyOrientation.BackLeft:
                anim.Play(animName + "_back");
                sr.flipX = true;
                return;
        }
    }

    public void CalculateOrientation(Vector2 direction)
    {
        if (direction.x >= 0)
        {
            if (direction.y <= 0)
            {
                Orientation = EnemyOrientation.FrontRight;
            }
            else
            {
                Orientation = EnemyOrientation.BackRight;
            }
        }
        else
        {
            if (direction.y <= 0)
            {
                Orientation = EnemyOrientation.FrontLeft;
            }
            else
            {
                Orientation = EnemyOrientation.BackLeft;
            }
        }
    }
}
