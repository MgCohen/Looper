using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType { Projectile, Ray}

[CreateAssetMenu(fileName = "New Gun", menuName = "Player/Gun")]
public class GunData : ScriptableObject
{
    public bool testing = false;
    [Header("Gun")]
    public Sprite weaponSprite;
    public Sprite aimSprite;

    [Header("Projectile")]
    public ShotType type;
    public GameObject bullet;
    public GameEvent muzzleFlash;

    [Header("Aiming")]
    public float minRange;
    public float maxRange;
    [Range(0f, 360f)]
    public float spread;

    [Header("Shotting")]
    public float shotCD;
    public float bulletsPerShot = 1;
    public int magazineSize = 1;
    public float reloadTime;
    [Range(0f, 1f)]
    public float speedVariance;

    [Header("Shot Configuration")]
    public int sequenceShooting = 1;
    public bool ammoPerShot = true;


    public List<Mods> defaultMods = new List<Mods>();

}
