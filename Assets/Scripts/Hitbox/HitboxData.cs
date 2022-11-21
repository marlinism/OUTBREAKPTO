using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageSource
{
    Neutral     = (1 << 0),
    Friendly    = (1 << 1),
    Hostile     = (1 << 2)
}

public enum DamageType
{
    None,
    Poke,
    Pierce,
    Slash,
    Slam
}

public enum DamageResponse
{
    None,
    Flinch,
    Trip,
    Launch
}

public class HitboxData
{
    public int Damage { get; set; }
    public DamageSource Source { get; set; }
    public DamageType Type { get; set; }
    public DamageResponse Response { get; set; }

    public HitboxData(int damage = 10,
            DamageSource source = DamageSource.Neutral,
            DamageType type = DamageType.None, 
            DamageResponse response = DamageResponse.None)
    {
        this.Damage = damage;
        this.Source = source;
        this.Type = type;
        this.Response = response;
    }
}