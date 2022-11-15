using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffectController : MonoBehaviour
{
    public float lifetime = 0.1f;

    private float startTime;

    // Start is called before the first frame update
    void Awake()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
