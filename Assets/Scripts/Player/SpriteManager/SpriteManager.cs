using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum AnimAction
{
    Null,
    Idle,
    Run,
    Roll
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
        get; set;
    }

    // Direction name of the animation
    public AnimDirection Direction
    {
        get; set;
    }

    // Defines the bodypart to play the animation
    public AnimBodyPart BodyPart
    {
        get; set;
    }

    // Indicate if the sprite should be X flipped
    public bool FlipX
    {
        get; set;
    }

    // X direction range for front and back direction detection
    public float frontBackRange = 0.4f;

    // Component references
    private SubspriteManager fullSM;
    private SubspriteManager topSM;
    private SubspriteManager bottomSM;

    // Start is called before the first frame update
    void Start()
    {
        fullSM = transform.Find("Full").GetComponent<SubspriteManager>();
        Assert.IsNotNull(fullSM);

        topSM = transform.Find("Top").GetComponent<SubspriteManager>();
        Assert.IsNotNull(topSM);

        bottomSM = transform.Find("Bottom").GetComponent<SubspriteManager>();
        Assert.IsNotNull(bottomSM);

        BodyPart = AnimBodyPart.Null;
        Action = AnimAction.Null;
        Direction = AnimDirection.Null;
        FlipX = false;
    }

    // Update the sprite animation given the current animation descriptors
    public void UpdateSprite()
    {
        string animationName = Action.ToString().ToLower() + "_" + Direction.ToString().ToLower();

        switch(BodyPart)
        {
            case AnimBodyPart.Full:
                fullSM.PlayAnimation("player_full_" + animationName, FlipX);
                topSM.Disable();
                bottomSM.Disable();
                return;

            case AnimBodyPart.Top:
                topSM.PlayAnimation("player_top_" + animationName, FlipX);
                break;

            case AnimBodyPart.Bottom:
                bottomSM.PlayAnimation("player_bottom_" + animationName, FlipX);
                break;

            case AnimBodyPart.TopBottom:
                topSM.PlayAnimation("player_top_" + animationName, FlipX);
                bottomSM.PlayAnimation("player_bottom_" + animationName, FlipX);
                break;

            case AnimBodyPart.Null:
            default:
                return;
        }

        fullSM.Disable();
    }

    // Play a specific animation
    public void PlayAnimation(string animationName)
    {
        if (animationName.Contains("_full_"))
        {
            fullSM.PlayAnimation(animationName, FlipX);
            topSM.Disable();
            bottomSM.Disable();
            return;
        }
        if (animationName.Contains("_top_"))
        {
            topSM.PlayAnimation(animationName, FlipX);
            fullSM.Disable();
            return;
        }
        if (animationName.Contains("_bottom_"))
        {
            bottomSM.PlayAnimation(animationName, FlipX);
            fullSM.Disable();
            return;
        }
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
