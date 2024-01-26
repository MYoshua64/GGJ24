using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Child : ControlledEntity
{
    [SerializeField] float pushTime = 0.35f;
    [SerializeField] float pushSpeedMultiplier = 5f;

    int scareCount;

    void Update()
    {
        HandleMovement();
        if (!isInLight)
        {
            scareCount++;
            if (scareCount >= 3)
            {
                Debug.Log("Game Over");
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
            else Push(-walkDirection);
        }
    }

    public async Task Push(Vector3 direction)
    {
        inControl = false;
        float startTime = Time.time;
        while (Time.time < startTime + pushTime)
        {
            characterController.Move(direction * Time.deltaTime * moveSpeedFactor * pushSpeedMultiplier);
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }
        inControl = true;
        if (isInLight) scareCount = 0;
    }
}
