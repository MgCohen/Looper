using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Yell", menuName = "Mods/Move/Yell")]
public class Yell : MoveMod
{
    public override void OnMove()
    {
        base.OnMove();
        Debug.Log("Moving");
    }

    public override void OnEndMove()
    {
        base.OnEndMove();
        Debug.Log("Moved");
    }
}
