using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public GunData gun;
    public Player player;
    public SpriteRenderer sprite;
    public Transform gunPoint;

    [Header("Targetting")]
    //public List<Transform> targets = new List<Transform>();
    public IDamageable target;
    Aim currentAim;
    float recharge = 0;

    private void OnEnable()
    {
        SetGun();
    }

    public void SetGun()
    {
        player = Player.instance;
        sprite.sprite = gun.weaponSprite;
    }

    public void Shot()
    {
        if (!gun || !player.Ready || player.Locked || target == null)
        {
            return;
        }
        if(recharge < gun.shotCD)
        {
            return;
        }
        foreach (var c in player.commands)
        {
            if (c.type == CommandType.Attacking)
                c.Do();
        }
        if (gun.type == ShotType.Projectile)
        {
            for (int i = 0; i < gun.bulletsPerShot; i++)
            {
                var speedVariance = Random.Range(1 - (gun.speedVariance/2), 1 + (gun.speedVariance / 2));
                var rotation = transform.rotation.eulerAngles;
                var variance = Random.Range(-gun.accuracy/2, gun.accuracy/2);
                rotation.z += variance;
                var obj = Lean.Pool.LeanPool.Spawn(gun.bullet, gunPoint.position, Quaternion.Euler(new Vector3(0, 0, rotation.z)));
                obj.GetComponent<Bullet>().speed *= speedVariance;
            }
        }
        recharge = 0;
    }
    public void Aim()
    {
        if (target != null && (target as MonoBehaviour) != null)
        {
            var point = (target as MonoBehaviour).transform.position;
            var z = Extensions.RotationZ(transform.position, point);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, z));
        }
    }
    private void Update()
    {
        Aim();
        recharge += Time.deltaTime;
    }
    public void SetTarget(Transform newTarget)
    {
        if (currentAim != null)
        {
            currentAim.DumpTarget();
        }
        if (newTarget == null)
        {
            target = null;
            return;
        }
        target = newTarget.GetComponent<IDamageable>();
        currentAim = (Lean.Pool.LeanPool.Spawn((GameObject)Resources.Load("Aim"))).GetComponent<Aim>();
        currentAim.SetTarget(newTarget, this);
    }
}
