using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponSpriteManager : MonoBehaviour
{
    public WeaponEquippedController wec;
    public Animator anim;
    public SpriteRenderer sr;

    private string weaponName;

    private bool visible;
    private bool flipped;
    AudioSource shootingSound;

    void Start() {
        shootingSound = GetComponent<AudioSource>();
		    shootingSound.volume = StateManager.voulumeLevel;
	}

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

    // Awake is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(wec);
        Assert.IsNotNull(anim);
        Assert.IsNotNull(sr);

        weaponName = wec.wm.weaponName;

        Hide();
    }

    public void PlayFireAnim()
    {
        shootingSound.Play();
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
