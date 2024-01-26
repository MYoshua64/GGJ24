using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float minDistance = 3.0f;
    [SerializeField] float maxDistance = 10.0f;

    ControlledEntity[] gameEntities;
    Vector3 midpoint;
    float halfDistance, halfFov;

    // Start is called before the first frame update
    void Start()
    {
        gameEntities = FindObjectsOfType<ControlledEntity>();
        halfFov = GetComponent<Camera>().fieldOfView * Mathf.Deg2Rad / 2.0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        midpoint = (gameEntities[0].transform.position + gameEntities[1].transform.position) / 2;
        halfDistance = Vector3.Magnitude(gameEntities[0].transform.position - gameEntities[1].transform.position) / 2;
        float desiredDistance = halfDistance / Mathf.Tan(halfFov);
        float actualDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        transform.position = midpoint - actualDistance * transform.forward;
    }
}
