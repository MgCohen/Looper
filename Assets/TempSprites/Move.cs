using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Move : MonoBehaviour
{
    public Vector3 offset;
    public float speed;


    private void OnEnable()
    {
        transform.DOMove(transform.position + offset, speed).SetSpeedBased().SetEase(Ease.Linear);
    }
}
