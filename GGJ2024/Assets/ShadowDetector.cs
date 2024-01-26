using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public Light lightSource;
    

    void Update()
    {
        // Get the point you want to check for shadows
        Vector3 pointToCheck = ControlledEntity.childInstance.transform.position;

        // Calculate the direction from the point to the light source
        Vector3 lightDirection = lightSource.transform.position - pointToCheck;

        // Create a ray from the point towards the light source
        Ray ray = new Ray(pointToCheck, lightDirection);

        // Set a maximum distance for the ray
        float maxDistance = Vector3.Distance(pointToCheck, lightSource.transform.position);

        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // If the ray hits an object, the point is in shadow
            Debug.Log("Point is in shadow");
        }
        else
        {
            // If the ray doesn't hit any object, the point is illuminated
            Debug.Log("Point is illuminated");
        }
    }
}
