using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : ControlledEntity
{
    
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        Debug.Log(isInLight);
        if (isInLight)
        {
            Debug.Log("Game Over");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
