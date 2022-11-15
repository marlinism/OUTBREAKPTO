using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerManager : MonoBehaviour
{
    // Component references
    public Rigidbody2D rb;
    public SpriteManager sm;
    public MoveStateManager msm;
    public WeaponInventoryManager wim;
    public PlayerInteractManager pim;

    // Speed of character
    public float movementSpeed = 4f;

    // Directional vectors for movement and character looking
    // Position for current mouse position
    private Vector3 moveDirection;
    private Vector3 lookDirection;
    private Vector3 pointPosition;

    // Player component properties
    public Rigidbody2D Rigidbody
    {
        get { return rb; } 
        set { rb = value; }
    }
    public SpriteManager Sprite
    {
        get { return sm; }
        set { sm = value; }
    }
    public MoveStateManager MoveState
    {
        get { return msm; }
        set { msm = value; }
    }
    public WeaponInventoryManager WeaponInventory
    {
        get { return wim; }
        set { wim = value; }
    }
    public PlayerInteractManager Interactions
    {
        get { return pim; }
        set { pim = value; }
    }

    // Directional and positional vector properties
    public Vector3 MoveDirection
    {
        get { return moveDirection; }
    }
    public Vector3 LookDirection
    {
        get { return lookDirection; }
    }
    public Vector3 PointPosition
    {
        get { return pointPosition; }
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
    void Awake()
    {
        Assert.IsNotNull(rb);
        Assert.IsNotNull(sm);
        Assert.IsNotNull(msm);
        Assert.IsNotNull(wim);

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

        wim.UpdateCurrentWeaponState();
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
        if (Input.GetButtonDown("Roll"))
        {
            msm.AddMoveState(new RollState());
        }

        if (Input.GetButtonDown("Interact"))
        {
            pim.Interact();
        }

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            wim.RotateNextWeapon();
        }
        else if (scroll < 0f)
        {
            wim.RotatePrevWeapon();
        }

        // TEMP FOR TESTING ONLY
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
