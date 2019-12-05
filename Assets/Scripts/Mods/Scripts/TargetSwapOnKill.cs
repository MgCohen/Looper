using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetKillSwap", menuName = "Mods/Bullet/KillSwap")]
public class TargetSwapOnKill : BulletMod
{

    public float swapRange;

    public override void OnKill(Bullet bullet)
    {
        Switch();
    }

    public void Switch()
    {
        Gun gun = null;
        foreach(var g in Player.instance.guns)
        {
            if (g.target.dead)
            {
                gun = g;
                break;
            }
        }
        var mask = LayerMask.GetMask("Enemy");
        var center = (gun.target as MonoBehaviour).transform.position;
        var possible = TargetSystem.FindTargets(center, swapRange, mask);
        var distance = Mathf.Infinity;
        Transform selected = null;
        foreach(var t in possible)
        {
            var dist = Vector2.Distance(t.position, center);
            var filter = LayerMask.GetMask("Obstacles");
            var LoS = !Physics2D.Raycast(center, (t.position - center), dist, filter);
            if(dist < distance && LoS)
            {
                distance = dist;
                selected = t;
            }
        }
        Player.instance.SetTarget(selected, gun);
    }
}
