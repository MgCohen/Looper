using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AttackCommand : Command
{
    public AttackEvent OnAttack = new AttackEvent();
    public AttackEvent OnEndAttack = new AttackEvent();

    public void Shot(Gun currentGun)
    {
        OnAttack.Invoke(currentGun);
    }

    public void AfterShot(Gun currentGun)
    {
        OnEndAttack.Invoke(currentGun);
    }
}

public class AttackEvent : UnityEvent<Gun>
{

}