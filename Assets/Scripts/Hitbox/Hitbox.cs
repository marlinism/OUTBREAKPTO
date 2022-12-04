using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Hitbox : MonoBehaviour
{
    public bool startEnabled = true;
    public int damage = 10;
    public DamageSource damageSource = DamageSource.Neutral;
    public DamageType damageType = DamageType.None;
    public DamageResponse damageReponse = DamageResponse.None;

    private Collider2D c2d;
    private HitboxData data;

    // Properties
    public HitboxData Data
    {
        get { return data; }
        set { data = value; }
    }
    public int Damage
    {
        get { return data.Damage; }
        set { data.Damage = value; }
    }
    public DamageSource Source
    {
        get { return data.Source; }
        set { data.Source = value; }
    }
    public DamageType Type
    {
        get { return data.Type; }
        set { data.Type = value; }
    }
    public DamageResponse Response
    {
        get { return data.Response; }
        set { data.Response = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        c2d = GetComponent<Collider2D>();
        Assert.IsNotNull(c2d);

        if (!startEnabled)
        {
            c2d.enabled = false;
        }

        data = new(damage, damageSource, damageType, damageReponse);
    }

    public void SetData(HitboxData newData)
    {
        data = newData;
    }

    public void Enable()
    {
        c2d.enabled = true;
    }

    public void Disable()
    {
        c2d.enabled = false;
    }
}
