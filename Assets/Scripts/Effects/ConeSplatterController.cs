using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ConeSplatterController : ParticleBurstEffect
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        transform.Rotate(Vector3.up, 90f);
    }
}
