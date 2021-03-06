﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


public class PlayerInput : MonoBehaviour
{

    Player player;

    private void OnEnable()
    {
        player = GetComponent<Player>();
        LeanTouch.OnFingerSwipe += Swipe;
        LeanTouch.OnFingerTap += Tap;
        LeanTouch.OnFingerUp += Hold;
        LeanTouch.OnFingerOld += Set;
        //LeanTouch.onfinger
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= Swipe;
        LeanTouch.OnFingerTap -= Tap;
        LeanTouch.OnFingerUp -= Hold;
        LeanTouch.OnFingerOld -= Set;
    }


    public void Swipe(LeanFinger finger)
    {
        var dir = finger.SwipeScaledDelta.normalized;
        var start = finger.GetStartWorldPosition(0);
        var end = finger.GetLastWorldPosition(0);
        foreach(var s in player.commands)
        {
            s.Swipe(dir, start, end);
        }
    }

    public void Tap(LeanFinger finger)
    {
        var pos = finger.GetLastWorldPosition(0);
        foreach(var s in player.commands)
        {
            s.Tap(pos);
        }
    }

    public void Hold(LeanFinger finger)
    {
        if (!finger.Old)
        {
            return;
        }
        var pos = finger.GetLastWorldPosition(0);
        var time = finger.Age;
        foreach (var s in player.commands)
        {
            s.Hold(pos, time);
        }
    }

    public void Set(LeanFinger finger)
    {
        if (!finger.Old)
        {
            return;
        }
        var pos = finger.GetLastWorldPosition(0);
        foreach(var s in player.commands)
        {
            s.Set(pos);
        }
    }
}
