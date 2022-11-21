using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyAlerter : MonoBehaviour
{
    [SerializeField]
    private Enemy targetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(targetEnemy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.HasTag("Player"))
        {
            targetEnemy.Alert(true);
            return;
        }
        if (collision.gameObject.HasTag("PlayerAlert"))
        {
            targetEnemy.Alert();
        }
    }
}
