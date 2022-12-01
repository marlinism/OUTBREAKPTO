using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponUnequippedController : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public InteractMarkerManager imm;

    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(rb2d);
        Assert.IsNotNull(imm);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        imm.Show();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        imm.Hide();
    }
}
