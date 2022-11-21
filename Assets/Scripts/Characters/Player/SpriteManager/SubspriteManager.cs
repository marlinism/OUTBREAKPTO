using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SubspriteManager : MonoBehaviour
{
    [SerializeField]
    protected string actorName;

    [SerializeField]
    protected string bodyPartName;

    // Component References
    protected Animator anim;
    protected SpriteRenderer sr;
    protected SpriteManager sm;

    // Indicate if animations are disabled
    protected bool disabled;

    // Indicate if the animator is locked
    protected bool locked;

    // Name of the previously performed action
    // Used for direction switch continuity
    protected string animNameLeader;
    protected string prevAction;
    protected string prevDirection;
    protected bool prevFlipped;

    // Default colors
    protected Color visible;
    protected Color invisible;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        Assert.IsNotNull(anim);

        sr = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(sr);

        sm = transform.parent.GetComponent<SpriteManager>();
        Assert.IsNotNull(sm);

        disabled = false;
        locked = false;

        animNameLeader = actorName.ToLower() + "_" + bodyPartName.ToLower();
        prevAction = "";
        prevDirection = "";
        visible = Color.white;
        invisible = Color.white;
        invisible.a = 0;
    }

    // Play a given animation with the indicated flipX
    public virtual void PlayAnimation(string action, string direction, bool flipX)
    {
        if (disabled)
        {
            sr.color = visible;
            disabled = false;
        }

        if (locked && action != prevAction)
        {
            return;
        }

        string animationName = animNameLeader + "_" + action + "_" + direction;

        // Check if need to perform direction switch continuity
        // (action animation is the same but just in a different direction)
        if (direction != prevDirection && action == prevAction)
        {
            float currAnimTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
            anim.Play(animationName, -1, currAnimTime + Time.deltaTime);
            prevDirection = direction;
        }
        else
        {
            anim.Play(animationName, -1, 0f);
            prevAction = action;
            prevDirection = direction;
        }

        sr.flipX = flipX;
        prevFlipped = flipX;
    }

    public virtual void UpdateDirection(string direction, bool flipX)
    {
        if (disabled)
        {
            return;
        }
        if (!(direction != prevDirection || flipX != prevFlipped))
        {
            return;
        }

        string animationName = animNameLeader + "_" + prevAction + "_" + direction;
        float currAnimTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        anim.Play(animationName, -1, currAnimTime + Time.deltaTime);
        sr.flipX = flipX;

        prevDirection = direction;
        prevFlipped = flipX;
    }

    // Lock the animator from playing other animations
    // A locked animator will not play an animation of a different action than previously played
    // Intended for use with SpriteManager's sequence animations
    public virtual void Lock()
    {
        locked = true;
    }

    // Release the lock on the animator from Lock()
    public virtual void Unlock()
    {
        locked = false;
        sm.MarkForUpdate();
    }

    // Disable animations for the subsprite
    // Reenabled upon calling PlayAnimation()
    public virtual void Disable()
    {
        anim.Play("disabled");
        prevAction = "disabled";
        sr.color = invisible;
        disabled = true;
    }
}
