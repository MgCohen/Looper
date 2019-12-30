using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase Target", menuName = "Mods/Player/Chase Target")]
public class ChaseTarget : PlayerMod
{
    public float speed;

    public override void Equip()
    {
        InstancedAction.RegisterUpdate(CheckTargets);
    }

    public override void Unequip()
    {
        InstancedAction.UnRegisterUpdate(CheckTargets);
    }

    public void CheckTargets()
    {
        foreach(var g in Player.instance.guns)
        {
            var t = (g.target as MonoBehaviour).transform;
            var dist = t.position - Player.instance.transform.position;
            Debug.Log(dist);
            if(dist.magnitude > g.gun.maxRange)
            {
                Debug.Log(1);
                Chase(dist.normalized);
            }
        }
    }

    public void Chase(Vector2 direction)
    {
        Player.instance.SetSpeed(direction * speed);
    }
}
