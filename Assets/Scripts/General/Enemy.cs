using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Enemy : MonoBehaviour, IDamageable
{
    public bool dead { get; set; }

    public AI ai;
    public int hp = 5;

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            if (!dead)
            {
                ClockSystem.ChangeScale(0.1f, 0.075f);
                Destroy(gameObject);
                //Lean.Pool.LeanPool.Despawn(gameObject, 5f);
                dead = true;
            }
        }
    }
}