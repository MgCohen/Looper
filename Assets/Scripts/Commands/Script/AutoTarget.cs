using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AutoTarget", menuName = "Command/Target/Auto")]
public class AutoTarget : TargetCommand
{

    public override void Equip()
    {
        base.Equip();
        InstancedAction.RegisterUpdate(Refresh);
    }

    public override void Unequip()
    {
        base.Unequip();
        InstancedAction.UnRegisterUpdate(Refresh);
    }

    public void Refresh()
    {
        if (!Player.instance.needTarget)
        {
            return;
        }
        var range = 0f;
        foreach(var g in Player.instance.guns)
        {
            if(g.gun.maxRange > range)
            {
                range = g.gun.maxRange;
            }
        }
        var point = Player.instance.transform.position;
        var mask = LayerMask.GetMask("Enemy");
        var target = TargetSystem.GetClosestTarget(point, range, mask, false);
        Player.instance.SetTarget(target);
    }
}
