using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileBaseColliderManger : MonoBehaviour
{
    private ProjectileController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<ProjectileController>();
        Assert.IsNotNull(pc);
    }

    public RaycastHit2D GetRaycast(float travelDist, int layerMask)
    {
        return Physics2D.Raycast(transform.position, transform.right, travelDist, layerMask);
    }
}
