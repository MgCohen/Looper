using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Third Shot Buff", menuName = "Mods/Attack/ThirdShot")]
public class ThirdShotBuff : AttackMod
{
    List<gunCounter> guns = new List<gunCounter>();

    public int ExtraBullets;
    public float ExtraSpread;
    public float ExtraCd;
    public float recoil;

    public bool LoadBefore = false;

    public override void OnShot(Gun gun)
    {
        if (!LoadBefore)
        {
            var counter = GetCounter(gun);
            counter.counter++;
            if (counter.counter >= 3)
            {
                SetShot(counter);
            }
            else if (counter.counter == 1 && counter.hasTimer == true)
            {
                
                counter.hasTimer = false;
                Debug.Log(1);
                counter.gun.gun.shotCD /= ExtraCd;
            }
        }
        else
        {
            var counter = GetCounter(gun);
            counter.counter++;
            if (counter.counter == 2)
            {
                counter.gun.gun.shotCD *= ExtraCd;
            }
            else if (counter.counter == 3)
            {
                counter.hasBullets = true;
                counter.gun.gun.bulletsPerShot *= ExtraBullets;
                counter.gun.gun.spread *= ExtraSpread;
                counter.gun.gun.shotCD /= ExtraCd;
                counter.counter = 0;
                var targetPoint = (counter.gun.target as MonoBehaviour).transform.position;
                var point = Player.instance.transform.position;
                Player.instance.ForceSpeed((point - targetPoint).normalized * recoil, 0.1f);
            }
            else if (counter.hasBullets)
            {
                counter.gun.gun.bulletsPerShot /= ExtraBullets;
                counter.gun.gun.spread /= ExtraSpread;
            }
        }
    }

    public override void AfterShot(Gun gun)
    {
        if (!LoadBefore)
        {
            var counter = GetCounter(gun);
            if (counter.counter >= 3)
            {
                ResetShot(counter);
            }
        }
    }

    public void SetShot(gunCounter gc)
    {
        gc.gun.gun.bulletsPerShot *= ExtraBullets;
        gc.gun.gun.shotCD *= ExtraCd;
        gc.hasBullets = true;
        gc.hasTimer = true;
        var targetPoint = (gc.gun.target as MonoBehaviour).transform.position;
        var point = Player.instance.transform.position;
        Player.instance.ForceSpeed((point - targetPoint).normalized * recoil, 0.1f);
    }

    public void ResetShot(gunCounter gc)
    {
        gc.gun.gun.bulletsPerShot /= ExtraBullets;
        gc.hasBullets = false;
        gc.counter = 0;
    }

    public gunCounter GetCounter(Gun gun)
    {
        foreach (var gc in guns)
        {
            if (gc.gun == gun)
            {
                return gc;
            }
        }
        var newCounter = new gunCounter(gun);
        guns.Add(newCounter);
        return newCounter;
    }

    public class gunCounter
    {
        public gunCounter(Gun newGun)
        {
            gun = newGun;
            counter = 0;
        }
        public Gun gun;
        public int counter;
        public bool hasBullets = false;
        public bool hasTimer = false;
    }
}
