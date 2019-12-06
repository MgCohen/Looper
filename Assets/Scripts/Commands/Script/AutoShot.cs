using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AutoShot", menuName = "Command/Attacking/AutoShot")]
public class AutoShot : AttackCommand
{
    public override void Equip()
    {
        InstancedAction.RegisterUpdate(TryShot);
    }

    public override void Unequip()
    {
        InstancedAction.UnRegisterUpdate(TryShot);
    }

    public void TryShot()
    {
        foreach(var g in Player.instance.guns)
        {
            g.Shot();
        }
    }
}
