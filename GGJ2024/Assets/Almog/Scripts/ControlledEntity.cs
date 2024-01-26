using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlledEntity : MonoBehaviour
{
    [SerializeField] float moveSpeedFactor = 3.0f;

    [SerializeField] Transform lightDetectionTransform;

    [SerializeField] float pushTime = 0.35f;
    [SerializeField] float pushSpeedMultiplier = 5f;

    public Vector3 lightDetectionPoint => lightDetectionTransform.position;
    Vector3 walkDirection;

    CharacterController characterController;
    bool inControl = true;
    int scareCount;

    public bool isInLight { get; set; }

    public static ControlledEntity childInstance;
    public static ControlledEntity monsterInstance;

    private void Awake()
    {
        if (gameObject.name.Contains("Child"))
        {
            childInstance = this;
        }
        else if (gameObject.name.Contains("Monster"))
        {
            monsterInstance = this;
        }
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.up * Physics.gravity.y * Time.deltaTime);
        }
        if (inControl)
        {
            if (movement != Vector3.zero)
            {
                characterController.Move(movement * moveSpeedFactor * Time.deltaTime);
            }
            if (isInLight)
            {
                if (gameObject.name == "Monster")
                {
                    Debug.Log("Game Over");
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
            }
            else if (gameObject.name == "Child")
            {
                scareCount++;
                if (scareCount == 3)
                {
                    Debug.Log("Game Over");
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
                else Push(-movement);
            }
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
