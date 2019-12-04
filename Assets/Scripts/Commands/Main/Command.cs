using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CommandType { Movement, Targetting, Attacking, Bullet, Other}
public class Command : ScriptableObject
{
    public CommandType type;

    [HideInInspector]
    public UnityEvent OnDo = new UnityEvent();
    [HideInInspector]
    public UnityEvent OnEnd = new UnityEvent();

    public virtual void Tap(Vector2 point)
    {

    }
    public virtual void Hold(Vector2 point, float time)
    {

    }
    public virtual void Swipe(Vector2 direction, Vector2 start, Vector2 finish)
    {

    }
    public virtual void Equip()
    {

    }
    public virtual void Unequip()
    {

    }
    public void Do()
    {
        OnDo.Invoke();
    }
    public void End()
    {
        OnEnd.Invoke();
    }

}
