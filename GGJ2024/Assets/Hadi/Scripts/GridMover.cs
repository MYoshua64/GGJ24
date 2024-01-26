using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

[SelectionBase]
public class GridMover : MonoBehaviour
{
    [SerializeField, Range(1, 3)] private int cellsPerMovement = 1;
    [SerializeField, Range(0.1f, 1.5f)] private float movementDuration = 0.7f;
    [SerializeField, Range(0.05f, 1f)] private float jumpPower = 0.15f;
    [SerializeField, Tooltip("Set the object's position to match a tile on the grid?")] 
    private bool setPositionOnStart = true;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private LayerMask obstacleLayerMask = 64;

    private bool canMove = true;
    private Grid grid;
    public Vector3Int GridPosition => grid.WorldToCell(transform.position);

    public bool CanMove => canMove;

    private void Start()
    {
        grid = tilemap.layoutGrid;
        if (setPositionOnStart)
        {
            Vector3Int worldToCell = GridPosition;
            transform.position = grid.GetCellCenterWorld(worldToCell);
        }
    }

    public void Move(Vector2 input)
    {
        if(!canMove) return;
        Vector3 direction;
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            direction = new Vector3(Mathf.Sign(input.x) , 0, 0);
        else
            direction = new Vector3(0, Mathf.Sign(input.y), 0);
      
        Vector3 movement = direction * cellsPerMovement;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction,movement.magnitude, obstacleLayerMask);
        if (raycastHit2D.collider != null) return;
        canMove = false;
        Vector3 target = transform.position + movement;
        transform.DOJump(target, jumpPower, 1, movementDuration).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            canMove = true;
        });
    }
    
    private void Reset()
    {
        if (tilemap == null)
            tilemap = FindObjectOfType<Tilemap>();
    }
}
