using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TapShot", menuName = "Command/Attacking/Tap Shot")]
public class TapShot : AttackCommand
{

    public override void Tap(Vector2 point)
    {
        TryShot();
    }

    public void TryShot()
    {
        foreach (var g in Player.instance.guns)
        {
            g.TryShot();
        }
    }
}
