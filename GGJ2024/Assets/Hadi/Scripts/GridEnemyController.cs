using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GridMover))]

public class GridEnemyController : MonoBehaviour, IGridController
{
    [SerializeField] private float timeBetweenMovements = 1.5f;
    private WaitForSeconds waitForSeconds;
    [SerializeField]
    private GridMover gridMover;
    private void Start()
    {
        Reset();
        waitForSeconds = new WaitForSeconds(timeBetweenMovements);
        StartCoroutine(RandomMovementCoroutine());
    }
    private void Reset()
    {
        gridMover = GetComponent<GridMover>();
    }

    IEnumerator RandomMovementCoroutine()
    {
        yield return waitForSeconds;
        while (true)
        {
            Move(new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)));
            yield return waitForSeconds;
        }
    }

    public void Move(Vector2 input)
    {
        gridMover.Move(input);
    }
}