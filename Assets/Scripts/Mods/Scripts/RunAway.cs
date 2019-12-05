using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Run Away", menuName = "Mods/Player/Run Away")]
public class RunAway : PlayerMod
{

    public float speed;
    public float range;

    bool running = false;

    public override void Equip()
    {
        InstancedAction.RegisterUpdate(Run);
    }

    public override void Unequip()
    {
        InstancedAction.UnRegisterUpdate(Run);
    }

    public void Run()
    {
        if (running)
        {
            var check = false;
            foreach (var g in Player.instance.guns)
            {
                var target = g.target as MonoBehaviour;
                if (target == null)
                {
                    return;
                }
                Vector2 point = Player.instance.transform.position;
                Vector2 targetPoint = target.transform.position;
                var dist = Vector2.Distance(point, targetPoint);
                if (dist <= range)
                {
                    check = true;
                    Run((point - targetPoint).normalized);
                }
            }
            if (!check)
            {
                running = false;
            }
        }
        else
        {
            foreach (var g in Player.instance.guns)
            {
                var target = g.target as MonoBehaviour;
                if (target == null)
                {
                    return;
                }
                Vector2 point = Player.instance.transform.position;
                Vector2 targetPoint = target.transform.position;
                var dist = Vector2.Distance(point, targetPoint);
                if (dist <= (range * 0.8f))
                {
                    running = true;
                    Run((point - targetPoint).normalized);
                }
            }
        }
    }

    public void Run(Vector2 direction)
    {
        Player.instance.SetSpeed(direction * speed);
    }
}
