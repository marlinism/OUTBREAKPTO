using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum ItemEffect
{
    Heal,
    UpgradeHealth
}

public class ItemDropController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private CircleCollider2D cc;

    [SerializeField]
    private ItemEffect effect;

    [SerializeField]
    private int healAmount = 50;

    [SerializeField]
    private int healthUpgradeAmount = 50;

    [SerializeField]
    private float collectDist = 0.2f;

    [SerializeField]
    private float attractionForce = 40f;

    private float maxDist;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(rb);

        maxDist = cc.radius + 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (maxDist <= 0 && collision.gameObject.HasTag("Player"))
        {
            maxDist = Vector3.Distance(transform.position, collision.transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent == null)
        {
            return;
        }

        GameObject player = collision.transform.parent.gameObject;
        if (!player.CompareTag("Player"))
        {
            return;
        }

        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist <= collectDist)
        {
            EnactEffect(player);
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (Vector2)player.transform.position - (Vector2)transform.position;
        direction.Normalize();

        float distForce = 1 - dist / maxDist;
        if (distForce < 0)
        {
            distForce = 0;
        }

        rb.AddForce(direction * (distForce * attractionForce), ForceMode2D.Force);
    }

    private void EnactEffect(GameObject playerObj)
    {
        PlayerManager player = playerObj.GetComponent<PlayerManager>();

        switch (effect)
        {
            case ItemEffect.Heal:
                player.Heal(healAmount);
                return;

            case ItemEffect.UpgradeHealth:
                player.IncreaseHealth(healthUpgradeAmount);
                return;
        }
    }
}
