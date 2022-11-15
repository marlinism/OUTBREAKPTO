using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Interactor : MonoBehaviour
{
    public Interactable target;

    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(target);
    }

    public void Interact()
    {
        target.Interact();
    }
}

