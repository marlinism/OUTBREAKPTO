using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BossProjectileController : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private SpriteRenderer projectileSr;

    [SerializeField]
    private SpriteRenderer shadowSr;

    [SerializeField]
    private Hitbox hitbox;

    [SerializeField]
    private GameObject potatoPrefab;

    [SerializeField]
    private float rotationSpeed = 180f;

    [SerializeField]
    private float movementSpeed = 9f;

    [SerializeField]
    private int minPotatoesSpawned = 2;

    [SerializeField]
    private int maxPotatoesSpawned = 5;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(anim);
        Assert.IsNotNull(projectileSr);
        Assert.IsNotNull(shadowSr);
        Assert.IsNotNull(hitbox);

        Color invisibleColor = Color.white;
        invisibleColor.a = 0f;

        shadowSr.color = invisibleColor;

        StartCoroutine(RiseCoroutine());
    }

    private IEnumerator RiseCoroutine()
    {
        float startTime = Time.time;
        float riseDuration = 0.5f;
        
        while (Time.time - startTime < riseDuration)
        {
            ApplyRotation();
            ApplyTranslation(true);

            yield return null;
        }

        startTime = Time.time;
        float fadeDuration = 0.5f;

        Color currColor = Color.white;

        while (Time.time - startTime < fadeDuration)
        {
            ApplyRotation();
            ApplyTranslation(true);

            currColor.a = 1 - ((Time.time - startTime) / fadeDuration);
            projectileSr.color = currColor;

            yield return null;
        }

        currColor.a = 0f;
        projectileSr.color = currColor;

        StartCoroutine(FallCoroutine());
        yield break;
    }

    private IEnumerator FallCoroutine()
    {
        GameObject player = PlayerSystem.Inst.GetPlayer();
        if (player != null)
        {
            transform.position = player.transform.position;
        }

        Vector3 projectilePos = Vector3.zero;
        projectilePos.y = movementSpeed * 1.5f;
        projectileSr.transform.localPosition = projectilePos;

        float startTime = Time.time;
        float fadeInTime = 1f;
        Color currColor = Color.white;
        currColor.a = 0f;

        while (Time.time - startTime < fadeInTime)
        {
            ApplyRotation();
            ApplyTranslation(false);

            currColor.a = (Time.time - startTime) / fadeInTime;
            projectileSr.color = currColor;
            shadowSr.color = currColor;
            yield return null;
        }

        while (projectileSr.transform.localPosition.y > 0.4f)
        {
            ApplyRotation();
            ApplyTranslation(false);

            if (projectileSr.transform.localPosition.y < 1.4f)
            {
                hitbox.Enable();
            }

            yield return null;
        }

        hitbox.Disable();

        projectilePos = new Vector3(0f, 0.4f, 0f);
        projectileSr.transform.localPosition = projectilePos;
        projectileSr.transform.rotation = Quaternion.identity;
        anim.Play("boss_projectile_open");
        yield return null;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        for (int i = Random.Range(minPotatoesSpawned, maxPotatoesSpawned); i > 0; --i)
        {
            GameObject instance = Instantiate(potatoPrefab);
            instance.transform.position = transform.position;
        }

        Destroy(gameObject);
        yield break;
    }

    private void ApplyRotation()
    {
        projectileSr.transform.localRotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.forward)
                    * projectileSr.transform.localRotation;
    }

    private void ApplyTranslation(bool upwards)
    {
        Vector3 displacement = Vector3.zero;
        displacement.y = movementSpeed * Time.deltaTime;

        if (upwards)
        {
            projectileSr.transform.position += displacement;
        }
        else
        {
            projectileSr.transform.position -= displacement;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
