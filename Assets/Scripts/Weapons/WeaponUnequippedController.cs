using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponUnequippedController : MonoBehaviour
{
    public Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(rb2d);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        // todo: highlight weapon sprite
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        // todo: unhighlight weapon sprite
    }
}
