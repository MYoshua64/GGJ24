using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : EnemyBase
{
    [SerializeField] Transform point1;
    [SerializeField] Transform point2;
    [SerializeField] float moveSpeed = 10.0f;

    Transform mesh;

    // Start is called before the first frame update
    void Start()
    {
        float moveTime = Vector3.Magnitude(point1.position - point2.position) / moveSpeed;
        transform.position = point1.position;
        transform.DOMove(point2.position, moveTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic).SetDelay(0.3f);
    }
}
