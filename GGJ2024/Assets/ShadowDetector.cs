using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public Light lightSource;

    ControlledEntity[] entities;

    private void Start()
    {
        entities = FindObjectsOfType<ControlledEntity>();
    }

    void Update()
    {
        DetectEntities();
    }

    void DetectEntities()
    {
        foreach (ControlledEntity entity in entities)
        {
            // Get the point you want to check for shadows
            Vector3 pointToCheck = entity.lightDetectionPoint;

            Debug.DrawLine(transform.position, pointToCheck);

            // Calculate the direction from the point to the light source
            Vector3 lightDirection = lightSource.transform.position - pointToCheck;

            // Create a ray from the point towards the light source
            Ray ray = new Ray(pointToCheck, lightDirection);

            // Set a maximum distance for the ray
            float maxDistance = Vector3.Distance(pointToCheck, lightSource.transform.position);

            // Perform the raycast
            RaycastHit hit;
            entity.isInLight = !Physics.Raycast(ray, out hit, maxDistance, ~LayerMask.GetMask(entity.gameObject.name));
            
        }
    }
}
