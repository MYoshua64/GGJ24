using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlledEntity : MonoBehaviour
{
    [SerializeField] protected float moveSpeedFactor = 3.0f;

    [SerializeField] Transform lightDetectionTransform;

    public Vector3 lightDetectionPoint => lightDetectionTransform.position;
    protected Vector3 walkDirection;
    protected CharacterController characterController;
    protected bool inControl = true;

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

    protected virtual void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    //protected virtual void Update()
    //{
    //    walkDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    //    if (!characterController.isGrounded)
    //    {
    //        characterController.Move(Vector3.up * Physics.gravity.y * Time.deltaTime);
    //    }
    //    if (inControl)
    //    {
    //        if (walkDirection != Vector3.zero)
    //        {
    //            characterController.Move(walkDirection * moveSpeedFactor * Time.deltaTime);
    //        }
    //    }
    //}

    protected void HandleMovement()
    {
        walkDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (walkDirection != Vector3.zero)
        {
            characterController.Move(walkDirection * moveSpeedFactor * Time.deltaTime);
        }
    }
}
