using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponSpriteManager : MonoBehaviour
{
    private string weaponName;

    private bool visible;
    private bool flipped;

    private WeaponController wc;
    private Animator anim;
    private SpriteRenderer sr;

    public bool Visible
    {
        get { return visible; }
        set
        {
            if (value)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }

    public bool Flipped
    {
        get { return flipped; }
        set
        {
            if (value)
            {
                flipped = true;
                sr.flipY = true;
            }
            else
            {
                flipped = false;
                sr.flipY = false;
            }
        }
    }

    public int SortOrder
    {
        get { return sr.sortingOrder; }
        set
        {
            sr.sortingOrder = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        wc = transform.parent.GetComponent<WeaponController>();
        Assert.IsNotNull(wc);

        anim = GetComponent<Animator>();
        Assert.IsNotNull(anim);

        sr = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(sr);

        weaponName = wc.weaponName;
        Visible = true;
    }

    public void PlayFireAnim()
    {
        anim.Play(weaponName + "_fire", -1, 0f);
    }

    public void FinishFireAnim()
    {
        anim.Play(weaponName + "_idle");
    }

    public void Show()
    {
        visible = true;
        sr.enabled = true;
    }

    public void Hide()
    {
        visible = false;
        sr.enabled = false;
    }
}
