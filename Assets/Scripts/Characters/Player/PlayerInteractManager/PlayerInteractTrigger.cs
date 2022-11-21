using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerInteractTrigger : MonoBehaviour
{
    public PlayerManager player;

    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(player);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.HasTag("Interaction"))
        {
            player.Interactions.AddInteractable(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.HasTag("Interaction"))
        {
            player.Interactions.RemoveInteractable(collision.gameObject);
        }
    }
}
