using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlledEntity : MonoBehaviour
{
    [SerializeField] private CharacterAnimationController animationController;
    [SerializeField] protected float moveSpeedFactor = 3.0f;

    [SerializeField] Transform lightDetectionTransform;

    public Vector3 lightDetectionPoint => lightDetectionTransform.position;
    protected Vector3 walkDirection;
    protected CharacterController characterController;
    protected bool inControl = true;

    public bool isInLight { get; set; }

    public static ControlledEntity childInstance;
    public static ControlledEntity monsterInstance;
    private Vector3 speedFactor;

    [Header("Pushing")]
    [SerializeField] private LayerMask pushableLayerMask = 512;
    [SerializeField] protected float pushRaycastDistance = 1f;
    [SerializeField] Transform raycastOrigin;

    public Vector3 SpeedFactor => speedFactor;
    
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
            if (walkDirection != Vector3.zero)
            {
                speedFactor = walkDirection * moveSpeedFactor * Time.deltaTime;
                characterController.Move(speedFactor);
            }
        }
        HandlePushing();
        if(animationController != null)
            animationController.UpdateAnimation(walkDirection.normalized);
    }

    protected void HandlePushing()
    {
        if (Physics.Raycast(raycastOrigin.position, SpeedFactor, out RaycastHit hitInfo, pushRaycastDistance, pushableLayerMask))
        {
            if (hitInfo.collider.TryGetComponent<MovableObject>(out MovableObject movableObject))
            {
                movableObject.Move(SpeedFactor, this);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(raycastOrigin.position, SpeedFactor.normalized * pushRaycastDistance);
    }
}
