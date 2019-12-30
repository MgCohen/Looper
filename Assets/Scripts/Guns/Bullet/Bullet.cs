using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifeTime;
    [HideInInspector] public IDamageable hitted;
    bool dead = false;

    public GameObject hitParticle;

    private void OnEnable()
    {
        Invoke("Die", lifeTime);
        TriggerShot();
    }
    public void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
    public virtual void Hit(Transform targetted)
    {
        hitted = targetted.GetComponent<IDamageable>();
        hitted.TakeDamage(damage);
        TriggerHit();
        if (hitted.dead) TriggerKill();
        Die();
    }
    public virtual void Die()
    {
        if (dead)
        {
            return;
        }
        //trigger animation
        dead = true;
        //Lean.Pool.LeanPool.Despawn(gameObject);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit(collision.transform);
    }

    public void TriggerShot()
    {
        var mods = Player.instance.mods;
        foreach (var m in mods)
        {
            if (m.targetType == CommandType.Bullet)
            {
                (m as BulletMod).OnShot(this);
            }
        }
    }

    public void TriggerHit()
    {
        var mods = Player.instance.mods;
        foreach (var m in mods)
        {
            if (m.targetType == CommandType.Bullet)
            {
                (m as BulletMod).OnHit(this);
            }
        }
    }

    public void TriggerKill()
    {
        var mods = Player.instance.mods;
        foreach (var m in mods)
        {
            if (m.targetType == CommandType.Bullet)
            {
                (m as BulletMod).OnKill(this);
            }
        }
    }
}
