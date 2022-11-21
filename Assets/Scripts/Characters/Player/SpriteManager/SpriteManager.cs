using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum AnimAction
{
    Null,
    Idle,
    Run,
    Roll,
    Flinch,
    CollapseDeath
}

public enum AnimDirection
{
    Null,
    SideFront,
    SideBack,
    Front,
    Back
}
public enum AnimBodyPart
{
    Null,
    Full,
    Top,
    Bottom,
    TopBottom
}

public class SpriteManager : MonoBehaviour
{
    // Action name of the animation
    public AnimAction Action
    {
        get { return action; }
        set
        {
            if (value != action && !locked)
            {
                action = value;
                needsUpdate = true;
            }
        }
    }

    // Direction name of the animation
    public AnimDirection Direction
    {
        get { return direction; } 
        set 
        {
            if (!locked)
            {
                direction = value;

            }
        }
    }

    // Defines the bodypart to play the animation
    public AnimBodyPart BodyPart
    {
        get { return bodyPart; }
        set
        {
            if (value != bodyPart && !locked)
            {
                bodyPart = value;
                needsUpdate = true;
            }
        }
    }

    // Indicate if the sprite should be X flipped
    public bool FlipX
    {
        get { return flipX; }
        set
        {
            if (!locked)
            {
                flipX = value;
            }
        }
    }

    // X direction range for front and back direction detection
    public float frontBackRange = 0.4f;

    // Animation component variables
    private AnimAction action;
    private AnimDirection direction;
    private AnimBodyPart bodyPart;
    private bool flipX;

    // Indicates if the sprite needs to be updated from a changed property
    private bool needsUpdate;

    private bool locked;

    // Component references
    private SpriteEffects sEffects;
    private SubspriteManager fullSM;
    private SubspriteManager topSM;
    private SubspriteManager bottomSM;

    // Properties
    public SpriteEffects Effects
    {
        get { return sEffects; }
    }

    // Start is called before the first frame update
    void Start()
    {
        sEffects = GetComponent<SpriteEffects>();
        Assert.IsNotNull(sEffects);

        fullSM = transform.Find("Full").GetComponent<SubspriteManager>();
        Assert.IsNotNull(fullSM);

        topSM = transform.Find("Top").GetComponent<SubspriteManager>();
        Assert.IsNotNull(topSM);

        bottomSM = transform.Find("Bottom").GetComponent<SubspriteManager>();
        Assert.IsNotNull(bottomSM);

        action = AnimAction.Null;
        direction = AnimDirection.Null;
        bodyPart = AnimBodyPart.Null;
        flipX = false;

        needsUpdate = true;
        locked = false;
    }

    // General function for updating the current animation or setting a new animation
    public void UpdateState()
    {
        if (needsUpdate)
        {
            PlayLoopAnimation();
            needsUpdate = false;
        }

        UpdateDirection();
    }

    // Mark that the animation needs to be updated when UpdateState() is called
    public void MarkForUpdate()
    {
        needsUpdate = true;
    }

    // Play a loop animation according to the set properties
    // The animation can be interrupted by any other animation type
    public void PlayLoopAnimation()
    {
        if (locked)
        {
            return;
        }

        switch(BodyPart)
        {
            case AnimBodyPart.Full:
                fullSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                topSM.Disable();
                bottomSM.Disable();
                return;

            case AnimBodyPart.Top:
                topSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                break;

            case AnimBodyPart.Bottom:
                bottomSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                break;

            case AnimBodyPart.TopBottom:
                topSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                bottomSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                break;

            case AnimBodyPart.Null:
            default:
                return;
        }

        fullSM.Disable();
    }

    // Play a sequence animation according to the set properties
    // Sequence animations cannot be interrupted unless OverrideSequences() is called or the animation completes
    public void PlaySequenceAnimation()
    {
        if (locked)
        {
            return;
        }

        switch (BodyPart)
        {
            case AnimBodyPart.Full:
                fullSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                fullSM.Lock();
                topSM.Disable();
                bottomSM.Disable();
                return;

            case AnimBodyPart.Top:
                topSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                topSM.Lock();
                break;

            case AnimBodyPart.Bottom:
                bottomSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                bottomSM.Lock();
                break;

            case AnimBodyPart.TopBottom:
                topSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                bottomSM.PlayAnimation(Action.ToString().ToLower(), Direction.ToString().ToLower(), FlipX);
                topSM.Lock();
                bottomSM.Lock();
                break;

            case AnimBodyPart.Null:
            default:
                return;
        }

        fullSM.Disable();
    }

    // Allow any sequence animations to be overridden by another animation
    public void OverrideSequences()
    {
        fullSM.Unlock();
        topSM.Unlock();
        bottomSM.Unlock();
    }

    // Prevents any modifications of the SpriteManager until Unlock() is called
    public void Lock()
    {
        locked = true;
    }

    // Reallows modification of the SpriteManager after Lock() is called
    public void Unlock()
    {
        locked = false;
    }

    // Update only the direction component of playing animations
    public void UpdateDirection()
    {
        string directionString = Direction.ToString().ToLower();

        fullSM.UpdateDirection(directionString, FlipX);
        topSM.UpdateDirection(directionString, FlipX);
        bottomSM.UpdateDirection(directionString, FlipX);
    }

    // Calculate the Direction animation descriptor given a direction vector
    public void CalculateDirection(Vector3 directionVector)
    {
        if (Mathf.Abs(directionVector.x) <= frontBackRange)
        {
            if (directionVector.y > 0)
            {
                Direction = AnimDirection.Back;
            }
            else
            {
                Direction = AnimDirection.Front;
            }
        }
        else
        {
            if (directionVector.y > 0)
            {
                Direction = AnimDirection.SideBack;
            }
            else
            {
                Direction = AnimDirection.SideFront;
            }
        }

        FlipX = (directionVector.x < -frontBackRange / 2);
    }
}
