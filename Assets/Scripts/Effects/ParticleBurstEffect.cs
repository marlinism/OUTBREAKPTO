using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBurstEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ps;

    public ParticleSystem ParticleComp
    {
        get { return ps; }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        StartDestroyTimer();
    }

    public void SetBustCount(int count)
    {
        ParticleSystem.EmissionModule emission = ps.emission;
        emission.burstCount = count;
    }

    public void SetSpeedRange(float minSpeed, float maxSpeed)
    {
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startSpeed = new ParticleSystem.MinMaxCurve(minSpeed, maxSpeed);
    }

    public void SetMaxSpeed(float speed)
    {
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startSpeed = new ParticleSystem.MinMaxCurve(speed / 4, speed);
    }

    public void SetSizeRange(float minSize, float maxSize)
    {
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);
    }

    public void SetMaxSize(float size)
    {
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startSize = new ParticleSystem.MinMaxCurve(0.03f, size);
    }

    public void StartDestroyTimer()
    {
        ParticleSystem.MainModule mainModule = ps.main;
        float maxLifetime = mainModule.startLifetime.constantMax;
        Destroy(gameObject, maxLifetime);
    }
}
