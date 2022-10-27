using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MuzzleFlashController : MonoBehaviour
{
    public GameObject closeLight = null;
    public GameObject farLight = null;
    public float lifeTime = 0.02f;

    private float spawnTime;
    private float closeLightIntensity;
    private float farLightIntensity;

    private Light2D closeLightComp;
    private Light2D farLightComp;


    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;

        closeLightComp = closeLight.GetComponent<Light2D>();
        farLightComp = farLight.GetComponent<Light2D>();

        closeLightIntensity = closeLightComp.intensity;
        farLightIntensity = farLightComp.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        float timeElapsed = Time.time - spawnTime;

        if (timeElapsed > lifeTime)
        {
            Destroy(gameObject);
        }

        float intensityPercent = 1 - (timeElapsed / lifeTime);
        closeLightComp.intensity = closeLightIntensity * intensityPercent;
        farLightComp.intensity = farLightIntensity * (1 - (timeElapsed / lifeTime));
    }
}
