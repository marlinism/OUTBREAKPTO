using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Assertions;

public enum ZoomLevel
{
    ZoomIn,
    Normal,
    ZoomOut1,
    ZoomOut2
}

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private PixelPerfectCamera pixelPerfect;

    [SerializeField]
    private AnimationCurve resizeCurve;

    // Target follow position of camera
    private Vector3 targetPosition;
    private float defaultZPosition = -10f;

    // Target gameobjects and follow strength
    // secondaryTargetStrength 0 = follow primary
    // secondaryTargetStrength 1 = follow secondary
    private GameObject primaryTarget;
    private GameObject secondaryTarget;
    private float secondaryTargetStrength = 0.5f;

    // Time values for moving the camera
    // Use to smooth sudden changes in target position
    private float transitionTime = 1f;
    private float currentTransitionTime = 0f;

    // Camera scaling information
    private ZoomLevel zoomScaleLevel = ZoomLevel.Normal;
    private Vector2 baseReferenceResolution;
    private float baseCameraSize;
    private float resizeTime = 1f;
    private float sizeScale = 1f;
    private float currentSizeScale = 1f;

    // Properties
    public GameObject PrimaryTarget
    {
        get { return primaryTarget; }
        set { SetPrimaryTarget(value); }
    }
    public GameObject SecondaryTarget
    {
        get { return secondaryTarget; }
        set { SetSecondaryTarget(value); }
    }
    public float SecondaryTargetStrength
    {
        get { return secondaryTargetStrength; }
        set { secondaryTargetStrength = value; }
    }
    public float TransitionTime
    {
        get { return transitionTime; }
        set { transitionTime = value; }
    }
    public float ResizeTime
    {
        get { return resizeTime; }
        set { resizeTime = value; }
    }
    public ZoomLevel ZoomScaleLevel
    {
        get { return zoomScaleLevel; }
        set { ChangeCameraSizeScale(zoomScaleLevel); }
    }
    public float SizeScale
    {
        get { return sizeScale; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(cam);
        Assert.IsNotNull(pixelPerfect);

        targetPosition = new(0f, 0f, -defaultZPosition);

        baseReferenceResolution = new();
        baseReferenceResolution.x = pixelPerfect.refResolutionX;
        baseReferenceResolution.y = pixelPerfect.refResolutionY;
        baseCameraSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug, remove later
        primaryTarget = PlayerSystem.Inst.GetPlayer();

        // Set current target position
        if (primaryTarget != null)
        {
            if (secondaryTarget != null)
            {
                targetPosition = Vector3.Lerp(primaryTarget.transform.position, 
                        secondaryTarget.transform.position, SecondaryTargetStrength);
                targetPosition.z = defaultZPosition;
            }
            else
            {
                targetPosition = primaryTarget.transform.position;
                targetPosition.z = defaultZPosition;
            }
        }

        // Move to target position
        Vector3 nextPosition;
        if (currentTransitionTime > 0)
        {
            nextPosition = Vector3.Lerp(transform.position, targetPosition, 
                    1 - (currentTransitionTime/transitionTime));
            currentTransitionTime -= Time.deltaTime;
        }
        else
        {
            nextPosition = targetPosition;
        }
        nextPosition.z = defaultZPosition;
        transform.position = nextPosition;
    }

    public void SetPrimaryTarget(GameObject target)
    {
        primaryTarget = target;
        currentTransitionTime = transitionTime;
    }

    public void SetSecondaryTarget(GameObject target)
    {
        secondaryTarget = target;
        currentTransitionTime = transitionTime;
    }

    public void ChangeCameraSizeScale(ZoomLevel zoomLevel)
    {
        switch (zoomLevel)
        {
            case ZoomLevel.ZoomIn:
                sizeScale = 0.8f;
                break;

            case ZoomLevel.Normal:
                sizeScale = 1f;
                break;

            case ZoomLevel.ZoomOut1:
                sizeScale = 4f / 3f;
                break;

            case ZoomLevel.ZoomOut2:
                sizeScale = 2f;
                break;
        }

        StopCoroutine(ChangeCameraSizeCoroutine());
        StartCoroutine(ChangeCameraSizeCoroutine());
    }

    private IEnumerator ChangeCameraSizeCoroutine()
    {
        pixelPerfect.enabled = false;
        float originalSize = currentSizeScale;
        float startTime = Time.time;
        while (Time.time - startTime < resizeTime)
        {
            float lerpPos = resizeCurve.Evaluate((Time.time - startTime) / resizeTime);
            currentSizeScale = Mathf.Lerp(originalSize, sizeScale, lerpPos);
            cam.orthographicSize = baseCameraSize * currentSizeScale;
            yield return null;
        }

        currentSizeScale = sizeScale;
        cam.orthographicSize = baseCameraSize * currentSizeScale;

        pixelPerfect.enabled = true;
        pixelPerfect.refResolutionX = (int)((float)baseReferenceResolution.x * currentSizeScale);
        pixelPerfect.refResolutionY = (int)((float)baseReferenceResolution.y * currentSizeScale);
        yield break;
    }
}
