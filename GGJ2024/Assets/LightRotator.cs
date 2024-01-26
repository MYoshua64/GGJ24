using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotator : MonoBehaviour
{
    [SerializeField] Transform pivot;
    float distance, angle;

    private void Start()
    {
        distance = Vector3.Distance(transform.position, pivot.position);
        Vector3 dirToPivot = (transform.position - pivot.position).normalized;
        angle = Vector3.SignedAngle(Vector3.forward, dirToPivot, Vector3.up);
    }

    private void Update()
    {
        float amountToMove = Input.GetAxisRaw("Rotate Light");

        if (Mathf.Abs(amountToMove) > 0 )
        {
            angle += amountToMove * 30 * Time.deltaTime;
            Vector3 desiredPos = pivot.position + Quaternion.Euler(Vector3.up * angle) * Vector3.forward * distance;
            transform.position = desiredPos;
        }
    }
}
