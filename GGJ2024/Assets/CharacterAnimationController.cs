using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private float animationSwitchTime = 0.4f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite frontIdle;
    [SerializeField] private Sprite[] frontWalk;
    [SerializeField] private Sprite[] rightWalk;
    [SerializeField] private Sprite[] leftWalk;
    [SerializeField] private Sprite[] backWalk;
    
    private enum Direction
    {
        Right,
        Left,
        Front,
        Back,
        Idle
    }

    [SerializeField]private Direction currentDirection;
    private WaitForSeconds waitForSeconds;
    [SerializeField]private int index = 0;
    [SerializeField]private Sprite[] currentAnimation;
    
    private void Start()
    {
        spriteRenderer.sprite = frontIdle;
        currentDirection = Direction.Idle;
        waitForSeconds = new WaitForSeconds(animationSwitchTime);
        StartCoroutine(AnimationSwitcher());
    }

    private IEnumerator AnimationSwitcher()
    {
        while (true)
        {
            yield return waitForSeconds;
            if (currentDirection != Direction.Idle)
            {
                index = (index + 1) % currentAnimation.Length;
                SetSprite();
            }
        }
    }

    private void SetSprite()
    {
        spriteRenderer.sprite = currentAnimation[index];
    }

    private void Update()
    {
        if (currentDirection == Direction.Idle)
            spriteRenderer.sprite = frontIdle;
    }

    public void UpdateAnimation(Vector3 movementDirection)
    {
        float horizontal = Mathf.Abs(movementDirection.x), vertical = Mathf.Abs(movementDirection.z);
        if (movementDirection == Vector3.zero)
        {
            currentDirection = Direction.Idle;
        }
        else
        {
            if (vertical > horizontal)
            {
                currentDirection = movementDirection.z > 0 ? Direction.Back : Direction.Front;
                currentAnimation = movementDirection.z > 0 ? backWalk : frontWalk;
            }
            else
            {
                currentDirection = movementDirection.x > 0 ? Direction.Right : Direction.Left;
                currentAnimation = movementDirection.x > 0 ? rightWalk : leftWalk;
            }
            SetSprite();
        }
    }
}