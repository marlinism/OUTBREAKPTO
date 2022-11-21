using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private ProjectileBaseColliderManger bcm;

    public GameObject impactEffect;

    public float speed = 50f;
    public int damage = 10;
    public DamageSource damageSource = DamageSource.Neutral;
    public DamageType damageType = DamageType.Pierce;
    public DamageResponse damageResponse = DamageResponse.Flinch;
    public float maxDistance = 50f;
    public float height = 0.5f;
    public bool penetrateThrough = false;

    private HitboxData damageInfo;
    private float totalDist;
    private bool mainColliderHit;
    private bool baseColliderHit;
    private int raycastMainMask;
    private int raycastBaseMask;
    private GameObject prevDamaged;

    public bool BaseColliderHit
    {
        get { return baseColliderHit; }
        set { baseColliderHit = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(bcm);

        damageInfo = new(damage, damageSource, damageType, damageResponse);
        totalDist = 0;
        mainColliderHit = false;
        baseColliderHit = false;
        raycastMainMask = (1 << LayerMask.NameToLayer("Obstacle")) | (1 << LayerMask.NameToLayer("Hurtbox"));
        raycastBaseMask = (1 << LayerMask.NameToLayer("Obstacle"));
        prevDamaged = null;

        Vector3 baseColliderPos = transform.position;
        baseColliderPos.y -= height;
        bcm.transform.position = baseColliderPos;
    }

    // Update is called once per frame
    void Update()
    {
        float travelDist = speed * Time.deltaTime;
        RaycastHit2D hitInfoMain = Physics2D.Raycast(transform.position, transform.right, travelDist, raycastMainMask);
        RaycastHit2D hitInfoBase = bcm.GetRaycast(travelDist, raycastBaseMask);

        // Check collisions
        //if (hitInfoMain.collider != null && !mainColliderHit)
        //{
        //    if (hitInfoMain.collider.tag == "Hittable")
        //    {
        //        if (!penetrateThrough || baseColliderHit)
        //        {
        //            CreateImpact(hitInfoMain);
        //            return;
        //        }

        //        // todo: call hittable collider's hit funtion

        //        mainColliderHit = true;
        //    }
        //    else if (hitInfoMain.collider.tag == "Obstacle")
        //    {
        //        if (baseColliderHit)
        //        {
        //            CreateImpact(hitInfoMain);
        //            return;
        //        }
        //        mainColliderHit = true;
        //    }
        //}

        //if (hitInfoBase.collider != null && hitInfoBase.collider.tag == "Obstacle" && !baseColliderHit)
        //{
        //    baseColliderHit = true;

        //    if (mainColliderHit)
        //    {
        //        CreateImpact(hitInfoBase);
        //        return;
        //    }
        //}

        // Check main collider
        if (hitInfoMain.collider != null)
        {
            if (hitInfoMain.collider.gameObject.HasTag("Damageable") && hitInfoMain.collider.gameObject != prevDamaged)
            {
                // Destroy collide if tag is projectile blocking or bullet cannot penetrate
                if (hitInfoMain.collider.gameObject.HasTag("ProjectileBlocking") || !penetrateThrough)
                {
                    CreateImpact(hitInfoMain);
                    return;
                }

                // Can damage through enemy
                Hurtbox targHitbox = hitInfoMain.collider.GetComponent<Hurtbox>();
                if (targHitbox != null)
                {
                    targHitbox.Hit(damageInfo, gameObject);
                    prevDamaged = hitInfoMain.collider.gameObject;
                }

                mainColliderHit = true;
            }
            else
            {
                if (baseColliderHit)
                {
                    CreateImpact(hitInfoMain);
                    return;
                }

                mainColliderHit = true;
            }
        }

        // Check base collider
        if (hitInfoBase.collider != null && !baseColliderHit)
        {
            baseColliderHit = true;

            if (mainColliderHit)
            {
                CreateImpact(hitInfoBase);
                return;
            }
        }

        transform.position += transform.right * travelDist;
        totalDist += travelDist;

        if (totalDist > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void CreateImpact(RaycastHit2D hitInfo)
    {
        transform.position += transform.right * hitInfo.distance;

        Hurtbox targHitbox = hitInfo.collider.GetComponent<Hurtbox>();
        if (targHitbox != null)
        {
            targHitbox.Hit(damageInfo, gameObject);
        }

        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect);
            effect.transform.position = transform.position;
            effect.transform.right = transform.right;
            StorageSystem.Inst.StoreEffect(effect);
        }

        Destroy(gameObject);
    }
}
