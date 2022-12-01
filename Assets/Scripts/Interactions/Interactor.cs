using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Interactor : MonoBehaviour
{
    public Interactable target;
    public InteractMarkerManager imm;

    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(target);
    }

    public void Interact()
    {
        target.Interact();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (imm != null)
        {
            imm.Show();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (imm != null)
        {
            imm.Hide();
        }
    }
}

