using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SubspriteManager : MonoBehaviour
{
    // Component References
    protected Animator anim;
    protected SpriteRenderer sr;

    // Indicate if animations are disabled
    protected bool disabled;

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

        disabled = false;
        visible = Color.white;
        invisible = Color.white;
        invisible.a = 0;
    }

    // Play a given animation with the indicated flipX
    public virtual void PlayAnimation(string animationName, bool flipX)
    {
        if (disabled)
        {
            sr.color = visible;
            disabled = false;
        }

        anim.Play(animationName);
        sr.flipX = flipX;
    }

    // Disable animations for the subsprite
    // Reenabled upon calling PlayAnimation()
    public virtual void Disable()
    {
        anim.Play("disabled");
        sr.color = invisible;
        disabled = true;
    }
}
