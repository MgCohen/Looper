using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetCommand : Command
{

    public TargetEvent OnSetTarget = new TargetEvent();
    public TargetEvent OnChangeTarget = new TargetEvent();

    public void SetTarget(Transform target)
    {
        OnSetTarget.Invoke(target);
    }

    public void ChangeTarget(Transform oldTarget)
    {
        OnChangeTarget.Invoke(oldTarget);
    }
}

public class TargetEvent: UnityEvent<Transform>
{

}
