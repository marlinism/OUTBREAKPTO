using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerManager : MonoBehaviour
{
    // Speed of character
    public float movementSpeed = 4f;

    // Directional vectors for movement and character looking
    // Position for current mouse position
    private Vector3 moveDirection;
    private Vector3 lookDirection;
    private Vector3 pointPosition;

    // Component references
    public Rigidbody2D rb
    {
        get; set;
    }
    public SpriteManager sm
    {
        get; set;
    }
    private MoveStateManager msm;
    private WeaponController wc;

    // Directional and positional vector properties
    public Vector3 MoveDirection
    {
        get
        {
            return moveDirection;
        }
    }
    public Vector3 LookDirection
    {
        get
        {
            return lookDirection;
        }
    }
    public Vector3 PointPosition
    {
        get
        {
            return pointPosition;
        }
    }

    // Control and Animation restriction properties
    public ControlRestriction ControlBlockLevel
    {
        get
        {
            return msm.ControlBlockLevel;
        }
    }
    public AnimationRestriction AnimationBlockLevel
    {
        get
        {
            return msm.AnimationBlockLevel;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb);

        sm = transform.Find("Move Offsetted/Sprite").GetComponent<SpriteManager>();
        Assert.IsNotNull(sm);

        msm = GetComponent<MoveStateManager>();
        Assert.IsNotNull(msm);

        wc = transform.Find("Move Offsetted/Weapon").GetComponent<WeaponController>();
        Assert.IsNotNull(wc);

        moveDirection = Vector3.zero;
        lookDirection = Vector3.zero;
        pointPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        GetMoveDirection();
        GetLookDirection();

        CheckActionInput();
        msm.Execute();

        UpdateVelocity();
        UpdateSprite();

        wc.UpdateWeaponState(lookDirection);
    }

    // Calculate moveDirection through the user's axis inputs
    private void GetMoveDirection()
    {
        if (ControlBlockLevel.HasFlag(ControlRestriction.Move))
        {
            return;
        }

        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.z = 0f;
        moveDirection.Normalize();
    }

    // Calculate lookDirection through the user's mouse position
    private void GetLookDirection()
    {
        if (ControlBlockLevel.HasFlag(ControlRestriction.Look))
        {
            return;
        }

        pointPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = pointPosition - transform.position;
        lookDirection.z = 0f;
        lookDirection.Normalize();
    }

    // Check for User inputs to perform player actions
    private void CheckActionInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            msm.AddMoveState(new RollState());
        }

        // stub
    }

    // Update the player's velocity given the value of moveDirection
    private void UpdateVelocity()
    {
        if (ControlBlockLevel.HasFlag(ControlRestriction.Move))
        {
            return;
        }

        if (moveDirection == Vector3.zero)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = moveDirection * movementSpeed;
    }

    // Update the player's sprite animation for basic movement
    private void UpdateSprite()
    {
        if (AnimationBlockLevel.HasFlag(AnimationRestriction.All))
        {
            return;
        }

        if (moveDirection == Vector3.zero)
        {
            sm.Action = AnimAction.Idle;
        }
        else
        {
            sm.Action = AnimAction.Run;
        }

        sm.CalculateDirection(lookDirection);

        if (!AnimationBlockLevel.HasFlag(AnimationRestriction.Top))
        {
            sm.BodyPart = AnimBodyPart.Top;
            sm.UpdateSprite();
        }
        if (!AnimationBlockLevel.HasFlag(AnimationRestriction.Bottom))
        {
            sm.BodyPart = AnimBodyPart.Bottom;
            sm.UpdateSprite();
        }
    }
}
