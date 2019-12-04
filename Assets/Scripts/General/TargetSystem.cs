using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    public static List<Transform> FindTargets(Vector2 center, float radius, LayerMask filter)
    {
        var targets = new List<Transform>();
        var hits = Physics2D.OverlapCircleAll(center, radius, filter);
        foreach(var hit in hits)
        {
            targets.Add(hit.transform);
        }
        return targets;
    }

    public static Transform GetClosestTarget(Vector2 center, float maxRange, LayerMask filter, bool needLOS = false)
    {
        var targets = FindTargets(center, maxRange, filter);
        Transform closest = null;
        float distance = Mathf.Infinity;
        foreach(var target in targets)
        {
            var dist = Vector2.Distance(target.position, center);
            if (needLOS)
            {
                var dir = (Vector2)target.position - center;
                var mask = LayerMask.GetMask("Obstacles");
                var LoS = !Physics2D.Raycast(center, dir, dist, mask);
                if (!LoS) continue;
            }
            if(dist < distance)
            {
                closest = target;
                distance = dist;
            }
        }
        return closest;
    }
}
