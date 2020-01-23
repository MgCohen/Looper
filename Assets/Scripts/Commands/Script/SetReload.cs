using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Command/Other/Set Reload")]
public class SetReload : OtherCommand
{

    public float emptyRange;
    public override void Set(Vector2 point)
    {

        var mask = LayerMask.GetMask("Enemy");
        var target = TargetSystem.GetClosestTarget(point, emptyRange, mask);
        if (target == null)
        {
            foreach (var g in Player.instance.guns)
            {
                g.Reload();
            }
        }
    }
}
