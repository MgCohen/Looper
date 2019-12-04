using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public bool dead { get; set; }

    public int hp = 5;

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            if (!dead)
                Destroy(gameObject);
                //Lean.Pool.LeanPool.Despawn(gameObject, 5f);
            dead = true;
        }
    }


}
