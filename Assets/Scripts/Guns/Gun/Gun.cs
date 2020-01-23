using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
    public float shotCD
    {
        get
        {
            var repetition = gun.sequenceShooting;
            if (repetition > 1) repetition++;
            return gun.shotCD * repetition;
        }
    }
    float recharge = 0;
    float reload = 0;

    public bool reloading
    {
        get
        {
            return (reload < gun.reloadTime);
        }
    }
    [HideInInspector] public int shotsFired = 0;
    //bool shotBlock;

    private void OnEnable()
    {
        SetGun();
    }
    public void SetGun()
    {
        player = Player.instance;
        sprite.sprite = gun.weaponSprite;
        reload = gun.reloadTime;
        var spriteBound = sprite.bounds.center.x + sprite.bounds.extents.x;
        gunPoint.position = new Vector2(spriteBound, gunPoint.position.y);
    }
    public void TryShot()
    {
        if (!gun || player.state != Player.State.Idle || target == null)
        {
            return;
        }
        if (recharge < shotCD || reloading)
        {
            return;
        }
        if (shotsFired >= gun.magazineSize && gun.magazineSize != -1)
        {
            Reload();
            return;
        }
        if (!gun.ammoPerShot)
        {
            shotsFired++;
        }
        recharge = 0;
        foreach (var c in player.commands)
        {
            if (c.type == CommandType.Attacking)
                (c as AttackCommand).Shot(this);
        }
        Shot(gun.sequenceShooting);
    }
    public void Shot(int amount)
    {
        if (gun.ammoPerShot)
        {
            if (shotsFired >= gun.magazineSize && gun.magazineSize != -1)
            {
                return;
            }
            shotsFired++;
        }
        if (player.state != Player.State.Idle)
        {
            return;
        }
        Camera.main.DOShakePosition(0.05f, 0.05f);
        if (gun.type == ShotType.Projectile)
        {
            for (int i = 0; i < gun.bulletsPerShot; i++)
            {
                var speedVariance = Random.Range(1 - (gun.speedVariance / 2), 1 + (gun.speedVariance / 2));
                var rotation = transform.rotation.eulerAngles;
                var variance = Random.Range(-gun.spread / 2, gun.spread / 2);
                rotation.z += variance;
                var obj = Lean.Pool.LeanPool.Spawn(gun.bullet, gunPoint.position, Quaternion.Euler(new Vector3(0, 0, rotation.z)));
                obj.GetComponent<Bullet>().speed *= speedVariance;
            }
        }

        amount--;
        if (amount >= 1)
        {
            InstancedAction.DelayAction(() =>
            {
                Shot(amount);
            }, gun.shotCD);
        }
        else
        {
            foreach (var c in player.commands)
            {
                if (c.type == CommandType.Attacking)
                    (c as AttackCommand).AfterShot(this);
            }
        }
    }
    public void Aim()
    {
        if (target != null && (target as MonoBehaviour) != null)
        {
            var point = (target as MonoBehaviour).transform.position;
            var z = Extensions.RotationZ(transform.position, point);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
            if (z < 0)
            {
                z = 360 - Mathf.Abs(z);
            }
            else if (z > 360)
            {
                z = z - 360;
            }
            sprite.flipY = (z < 90 || z > 270) ? false : true;
            //sprite.sortingOrder = (z > 0 && z < 180)? -1: 1;
            if ((target as MonoBehaviour).transform.position.x < player.transform.position.x)
            {
                player.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                player.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    private void Update()
    {
        Aim();
        recharge += Time.deltaTime;
        reload += Time.deltaTime;
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
        Aim();
    }
    public void Reload()
    {
        shotsFired = 0;
        reload = 0;
        //reload animation
        var rotateTime = Mathf.Clamp(gun.reloadTime * 0.75f, 0f, 0.35f);
        sprite.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 360), rotateTime, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    }
}

