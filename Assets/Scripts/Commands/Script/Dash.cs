using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Command/Move/Dash")]
public class Dash : MoveCommand
{
    public float speed;
    public override void Swipe(Vector2 direction, Vector2 start, Vector2 finish)
    {
        Do();
        var player = Player.instance;
        player.transform.position += (Vector3)direction * speed;
        End();
    }
}
