using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Command/Move/Dash")]
public class Dash : MoveCommand
{
    public float speed;
    public float staminaCost;
    public override void Swipe(Vector2 direction, Vector2 start, Vector2 finish)
    {
        if (staminaCost > Player.instance.stamina)
        {
            return;
        }
        Player.instance.stamina -= staminaCost;
        Move(Player.instance.transform.position);
        var body = Player.instance.body;
        body.AddForce(direction * speed, ForceMode2D.Impulse);
        InstancedAction.DelayAction(() => { EndMove(Player.instance.transform.position); }, 0.25f);
    }

}
