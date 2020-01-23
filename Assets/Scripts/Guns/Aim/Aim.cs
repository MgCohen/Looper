using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    Transform target = null;

    SpriteRenderer sprite;

    private void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void SetTarget(Transform newTarget, Gun sourceGun)
    {
        //spawn animation
        //change size to match
        if(newTarget == null)
        {
            return;
        }
        sprite.sprite = sourceGun.gun.aimSprite;
        target = newTarget;
        var bounds = target.GetBounds();
        transform.position = bounds.center;
    }

    public void DumpTarget()
    {
        //dump animation
        Lean.Pool.LeanPool.Despawn(gameObject);
        //Destroy(gameObject);
    }

    private void Update()
    {
        if(target == null)
        {
            return;
        }
        var bounds = target.GetBounds();
        transform.position = bounds.center;
    }
}
