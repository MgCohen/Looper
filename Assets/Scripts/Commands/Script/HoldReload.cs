using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Command/Other/Hold Reload")]
public class HoldReload : OtherCommand
{
    public float holdTime;
    public override void Hold(Vector2 point, float time)
    {
        if(time >= holdTime)
        {
            foreach(var g in Player.instance.guns)
            {
                g.Reload();
            }
        }
    }
}
