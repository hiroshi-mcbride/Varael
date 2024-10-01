using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]
public class PlatformMovement : MonoBehaviour
{
    public float walkSpeed, runSpeed, rotationSpeed, minJumpHeight, maxJumpHeight, jumpDuration, fallDuration, coyoteTime, jumpBufferDuration, horizontalInertia;
    public GroundCheck groundCheck;
    
    private float hMovementSpeed, jumpGravity, fallGravity, minJumpVelocity, maxJumpVelocity, currentCoyoteTime;
    private Rigidbody rb;
    private Vector3 velocity;
    private Quaternion rotation;

    private bool jumpQueued, jumpReleaseQueued, isJumping;

    public Camera cam;
    private Transform camTransform;
    private PlayerMovementInputAsset movementInputAsset;

    private Vector2 inputVector;
    private Vector3 relativeInput;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpGravity = 2f * maxJumpHeight / Mathf.Pow(jumpDuration, 2);
        fallGravity = maxJumpHeight / Mathf.Pow(fallDuration, 2);
        minJumpVelocity = Mathf.Sqrt(2 * jumpGravity * minJumpHeight);
        maxJumpVelocity = Mathf.Sqrt(2 * jumpGravity * maxJumpHeight);

        if (cam != null)
        {
            camTransform = cam.transform;
        }

        movementInputAsset = new PlayerMovementInputAsset();
        movementInputAsset.Player.Jump.started += _ctx => QueueJump();
        movementInputAsset.Player.Jump.canceled += _ctx => QueueJumpRelease();
    }

    private void QueueJump()
    {
        if (groundCheck.isGrounded)
        {
            jumpQueued = true;
        }
    }

    private void QueueJumpRelease()
    {
        if (isJumping)
        {
            jumpReleaseQueued = true;
        }
    }

    private void Update()
    {
        inputVector = movementInputAsset.Player.Walk.ReadValue<Vector2>();
        Vector3 forward = new Vector3(camTransform.forward.x, .0f, camTransform.forward.z).normalized;
        Vector3 right = new Vector3(camTransform.right.x, .0f, camTransform.right.z).normalized;
        relativeInput = forward * inputVector.y + right * inputVector.x;

    }

    
    private void FixedUpdate()
    {
        velocity = rb.linearVelocity;
        HandleHorizontalMovement();
        HandleGravity();
        UpdateGrounded();
        
        if (jumpQueued && groundCheck.isGrounded)
        {
            velocity = new Vector3(velocity.x, maxJumpVelocity, velocity.z);
            isJumping = true;
            jumpQueued = false;
        }
        
        if (jumpReleaseQueued)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity = new Vector3(velocity.x, minJumpVelocity, velocity.z);
            }

            jumpReleaseQueued = false;
        }
        rb.MoveRotation(rotation);
        rb.linearVelocity = velocity;
    }

    
    private void OnEnable()
    {
        movementInputAsset.Player.Enable();
    }

    
    private void OnDisable()
    {
        movementInputAsset.Player.Disable();
    }

    
    private void HandleHorizontalMovement()
    {
        hMovementSpeed = walkSpeed;
        
        if (inputVector.sqrMagnitude > .0f)
        {
            float angle = Mathf.Atan2(relativeInput.x, relativeInput.z) * Mathf.Rad2Deg;
            Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.up);
            rotation = Quaternion.Lerp(rb.rotation, newRotation, .2f);
        }
        else
        {
            rotation = rb.rotation;
        }        
        
        Vector3 newSpeed = Vector3.Lerp(velocity, relativeInput * hMovementSpeed, horizontalInertia);
        velocity = new Vector3(newSpeed.x, velocity.y, newSpeed.z);
    }
    
    
    private void UpdateGrounded()
    {
        if (!groundCheck.isGrounded)
        {
            return;
        }

        float distance = groundCheck.GetNearestDistance();
        if (distance > .0f)
        {
            rb.MovePosition(transform.position + Vector3.down * distance);
            if (velocity.y < .0f)
            {
                isJumping = false;
            }
        }

        if (!isJumping)
        {
            velocity = new Vector3(velocity.x, .0f, velocity.z);
        }
    }

    
    private void HandleGravity()
    {
        if (velocity.y >= 0)
        {
            velocity = new Vector3(velocity.x, velocity.y - jumpGravity * Time.fixedDeltaTime, velocity.z);
        }
        else
        {
            velocity = new Vector3(velocity.x, velocity.y - fallGravity * Time.fixedDeltaTime, velocity.z);
        }
    }

    
    public bool IsMoving()
    {
        return Mathf.Abs(velocity.sqrMagnitude) > 1.0f;
    }
}
