using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DoorController : Interactable
{
    public Animator anim;
    public string doorName;
    public bool startLocked = false;
    public bool toggleClose = false;

    private bool locked;
    private bool opened;

    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(anim);
        Assert.IsNotNull(doorName);

        locked = startLocked;
        opened = false;
    }

    public override void Interact()
    {
        if (locked)
        {
            return;
        }

        if (!opened)
        {
            anim.Play(doorName + "_opening");
            opened = true;
        }
        else if (toggleClose)
        {
            anim.Play(doorName + "_closing");
            opened = false;
        }
    }

    public void Open()
    {
        if (opened)
        {
            return;
        }

        anim.Play(doorName + "_opening");
        opened = true;
    }

    public void Close()
    {
        if (!opened)
        {
            return;
        }

        anim.Play(doorName + "_closing");
        opened = false;
    }
}
