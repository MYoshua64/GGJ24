using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovableObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool movedByChild = true;
    [SerializeField] private bool movedByMonster = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 movement, ControlledEntity pusher)
    {
        Debug.Log(pusher, this);
        if ((movedByChild && pusher is Child) || (movedByMonster && pusher is Monster))
        {
            rb.MovePosition(transform.position + movement);
        }
    }


}
