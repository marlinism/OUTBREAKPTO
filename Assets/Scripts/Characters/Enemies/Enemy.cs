using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct SplatterEffects
{
    public GameObject defaultEffect;
    public GameObject pokeEffect;
    public GameObject pierceEffect;
    public GameObject slashEffect;
    public GameObject slamEffect;
}

public abstract class Enemy : Damageable
{
    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    private GameObject alertTrigger;

    [SerializeField]
    private GameObject playerAlertSignal;

    [SerializeField]
    private SplatterEffects effectsList;

    [SerializeField]
    private bool startAlerted = false;

    [SerializeField]
    private Vector2 startingDirection = Vector2.zero;

    [SerializeField]
    protected float speed = 3f;

    // Direction the enemy is currently facing
    // Used for alert detection
    protected Vector2 faceDirection;

    // Indicates if the enemy has been alerted to the player
    // An alerted enemy knows the player's location and will attack them
    protected bool alerted;

    // Reference to the active player
    // Only initialized when the enemy is alerted
    private PlayerManager player;

    // Mask for SightlineToPlayer() method
    private int raycastMask;

    // Properties
    public Vector2 FaceDirection
    {
        get { return faceDirection; }
        set { faceDirection = value; }
    }
    public bool Alerted
    {
        get { return alerted; }
    }
    public PlayerManager Player
    {
        get { return player; }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb);
        Assert.IsNotNull(alertTrigger);
        Assert.IsNotNull(playerAlertSignal);

        raycastMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Obstacle"));

        startingDirection.Normalize();
        if (startingDirection == Vector2.zero)
        {
            faceDirection = new();
            faceDirection.x = Random.Range(-1f, 1f);
            faceDirection.y = Random.Range(-1f, 1f);

            if (faceDirection == Vector2.zero)
            {
                faceDirection.x = 1f;
            }
            faceDirection.Normalize();
        }
        else
        {
            faceDirection = startingDirection;
        }

        if (!startAlerted || !Alert())
        {
            player = null;
            alerted = false;
        }
    }

    // Damageable RecieveDamage method implementation
    public override void RecieveDamage(HitboxData damageInfo, GameObject collider = null)
    {
        currHealth -= damageInfo.Damage;

        switch (damageInfo.Type)
        {
            case DamageType.None:
                SpawnEffect(effectsList.defaultEffect, damageInfo, collider);
                break;

            case DamageType.Poke:
                SpawnEffect(effectsList.pokeEffect, damageInfo, collider);
                break;

            case DamageType.Pierce:
                SpawnEffect(effectsList.pierceEffect, damageInfo, collider);
                break;

            case DamageType.Slash:
                SpawnEffect(effectsList.slashEffect, damageInfo, collider);
                break;

            case DamageType.Slam:
                SpawnEffect(effectsList.slamEffect, damageInfo, collider);
                break;
        }

        if (currHealth <= 0)
        {
            Kill();
        }

        if (!alerted && damageInfo.Source == DamageSource.Friendly)
        {
            Alert(true);
        }
    }

    // Alert the enemy if the player is nearby
    public virtual bool Alert(bool createAlertSignal = false)
    {
        player = PlayerSystem.Inst.GetPlayerManager();
        if (player == null)
        {
            return false;
        }

        if (createAlertSignal && playerAlertSignal != null)
        {
            GameObject signal = Instantiate(playerAlertSignal);
            signal.transform.position = transform.position;
            Destroy(signal, 2f);
        }

        alertTrigger.SetActive(false);
        alerted = true;
        return true;
    }

    // Remove the alerted signifier for the enemy
    public virtual void Unalert()
    {
        alertTrigger.SetActive(true);
        alerted = false;
        player = null;
    }

    // Move the enemy towards its facing direction
    public void MoveTowardsFaceDirection()
    {
        rb.velocity = faceDirection * speed;
    }

    // Move the enemy towards a coordinate point
    public void MoveTowardsPoint(Vector2 point)
    {
        Vector2 moveDirection = point - (Vector2)transform.position;
        moveDirection.Normalize();

        rb.velocity = moveDirection * speed;
    }

    // Stop the movement of the enemy (set velocity to zero)
    public void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

    // Set the enemy's facing direction towards the player
    public void FaceTowardsPlayer()
    {
        if (player == null)
        {
            return;
        }

        faceDirection = player.transform.position - transform.position;
        faceDirection.Normalize();
    }

    // Get a direction vector pointing towards the player
    public Vector2 DirectionTowardsPlayer()
    {
        if (player == null)
        {
            return Vector2.zero;
        }

        return DirectionTowardsPoint(player.transform.position);
    }

    // Get a direction vector pointing towards a coordinate point
    public Vector2 DirectionTowardsPoint(Vector2 point)
    {
        return point - (Vector2)transform.position;
    }

    // Get the distance from the player in units
    public float DistanceFromPlayer()
    {
        if (player == null)
        {
            return -1f;
        }

        return DistanceFromPoint(player.transform.position);
    }

    // Get the distance from a coordinate point
    public float DistanceFromPoint(Vector2 point)
    {
        return Vector2.Distance((Vector2)transform.position, point);
    }

    // Check if there is a sightline to the player
    public bool SightlineToPlayer()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,
                DirectionTowardsPlayer(), Mathf.Infinity, raycastMask);

        if (hitInfo.collider == null || !hitInfo.collider.CompareTag("Player"))
        {
            return false;
        }

        return true;
    }

    // Get the distance of the nearest collision towards the given direction
    public float ClosestCollision(Vector2 direction)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,
                direction, Mathf.Infinity, raycastMask);

        return hitInfo.distance;
    }

    private void SpawnEffect(GameObject effect, HitboxData hitInfo, GameObject collider)
    {
        if (effect == null)
        {
            return;
        }

        GameObject spawnedEffect = Instantiate(effect);
        StorageSystem.Inst.StoreEffect(spawnedEffect);

        if (collider != null)
        {
            spawnedEffect.transform.position = collider.transform.position;
            spawnedEffect.transform.right = collider.transform.right;
        }
        else
        {
            spawnedEffect.transform.position = transform.position;
        }
    }
}
