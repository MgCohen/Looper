using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Command/Move/Dash")]
public class Dash : MoveCommand
{
    public float speed;
    public float staminaCost;
    public float duration;
    public float invincibilityFrames;
    public bool forceBased;

    public override void Swipe(Vector2 direction, Vector2 start, Vector2 finish)
    {
        if (staminaCost > Player.instance.stamina)
        {
            return;
        }
        Player.instance.Act("Dash");
        Player.instance.state = Player.State.Dashing;
        Player.instance.stamina -= staminaCost;
        Move(Player.instance.transform.position);
        //var body = Player.instance.body;
        //body.AddForce(direction * speed, ForceMode2D.Impulse);
        Player.instance.ForceSpeed(direction.normalized * speed, duration, false, forceBased);
        InstancedAction.DelayAction(() => { EndMove(Player.instance.transform.position); Player.instance.state = Player.State.Idle; }, duration);
        //set invincibility frames
    }

}
