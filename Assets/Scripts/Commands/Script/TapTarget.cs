﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Command/Target/Tap Target")]
public class TapTarget : TargetCommand
{

    public float targetRange;

    public override void Tap(Vector2 point)
    {
        var mask = LayerMask.GetMask("Enemy");
        var target = TargetSystem.GetClosestTarget(point, targetRange, mask);
        if (target != null)
        {
            Player.instance.SetTarget(target);
            SetTarget(target);
        }
    }


}
