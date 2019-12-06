using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Yell", menuName = "Mods/Move/Yell")]
public class Yell : MoveMod
{
    public override void OnMove(Vector2 point)
    {
        Debug.Log("Moving");
    }

    public override void OnEndMove(Vector2 point)
    {
        Debug.Log("Moved");
    }
}
