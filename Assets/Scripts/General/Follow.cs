using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    Transform target;

    private void Start()
    {
        target = Player.instance.transform;
    }

    private void Update()
    {
        var point = target.GetBounds().center;
        transform.position = point;
    }
}
