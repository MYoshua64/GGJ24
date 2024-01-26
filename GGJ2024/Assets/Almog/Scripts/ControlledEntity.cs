using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ControlledEntity : MonoBehaviour
{
    [SerializeField] float moveSpeedFactor = 3.0f;

    CharacterController characterController;
    bool inControl = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.up * Physics.gravity.y * Time.deltaTime);
        }
        if (!inControl) return;
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (movement != Vector3.zero)
        {
            characterController.Move(movement * moveSpeedFactor * Time.deltaTime);
        }
    }

    public async Task Push(Vector3 direction)
    {
        Debug.Log($"{gameObject.name} should be pushed!");
        inControl = false;
        float startTime = Time.time;
        while (Time.time < startTime + 0.35f)
        {
            characterController.Move(direction * Time.deltaTime * moveSpeedFactor * 5f);
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }
        inControl = true;
    }
}
