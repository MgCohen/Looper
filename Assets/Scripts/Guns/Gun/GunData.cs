using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType { Projectile, Ray}

[CreateAssetMenu(fileName = "New Gun", menuName = "Player/Gun")]
public class GunData : ScriptableObject
{
    public ShotType type;

    public Sprite weaponSprite;
    public Sprite aimSprite;

    public GameObject bullet;

    public float minRange;
    public float maxRange;
    [Range(0f, 360f)]
    public float accuracy;
    [Range(0f, 1f)]
    public float speedVariance;

    public float shotCD;
    public float bulletsPerShot;

    public List<Mods> defaultMods = new List<Mods>();

}
