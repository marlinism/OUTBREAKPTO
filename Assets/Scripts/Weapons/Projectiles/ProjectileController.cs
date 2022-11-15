using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileController : MonoBehaviour
{
    public GameObject impactEffect;

    public float speed = 50f;
    public float maxDistance = 50f;
    public float height = 0.5f;
    public bool penetrateThrough = false;

    private float totalDist;
    private bool mainColliderHit;
    private bool baseColliderHit;

    private ProjectileBaseColliderManger bcm;

    public bool BaseColliderHit
    {
        get { return baseColliderHit; }
        set { baseColliderHit = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        bcm = transform.Find("Base Collider").GetComponent<ProjectileBaseColliderManger>();
        Assert.IsNotNull(bcm);

        totalDist = 0;
        mainColliderHit = false;
        baseColliderHit = false;

        Vector3 baseColliderPos = transform.position;
        baseColliderPos.y -= height;
        bcm.transform.position = baseColliderPos;
    }

    // Update is called once per frame
    void Update()
    {
        float travelDist = speed * Time.deltaTime;
        int layerMask = (1 << LayerMask.NameToLayer("Obstacle")) | (1 << LayerMask.NameToLayer("Hittable"));
        RaycastHit2D hitInfoMain = Physics2D.Raycast(transform.position, transform.right, travelDist, layerMask);
        RaycastHit2D hitInfoBase = bcm.GetRaycast(travelDist, layerMask);

        // Check collisions
        if (hitInfoMain.collider != null && !mainColliderHit)
        {
            if (hitInfoMain.collider.tag == "Hittable")
            {
                if (!penetrateThrough || baseColliderHit)
                {
                    CreateImpact(hitInfoMain);
                    return;
                }

                // todo: call hittable collider's hit funtion

                mainColliderHit = true;
            }
            else if (hitInfoMain.collider.tag == "Obstacle")
            {
                if (baseColliderHit)
                {
                    CreateImpact(hitInfoMain);
                    return;
                }
                mainColliderHit = true;
            }
        }

        if (hitInfoBase.collider != null && hitInfoBase.collider.tag == "Obstacle" && !baseColliderHit)
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

        // todo: call collider's hit function

        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect);
            effect.transform.position = transform.position;
            effect.transform.right = transform.right;
        }

        Destroy(gameObject);
    }
}
