using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveCommand : Command
{
    public MoveEvent OnMove = new MoveEvent();
    public MoveEvent OnEndMove = new MoveEvent();

    public void Move(Vector2 currentPoint)
    {
        OnMove.Invoke(currentPoint);
    }

    public void EndMove(Vector2 currentPoint)
    {
        OnEndMove.Invoke(currentPoint);
    }
}

public class MoveEvent: UnityEvent<Vector2>
{

}
