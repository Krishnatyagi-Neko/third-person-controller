using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;
    Rigidbody playerrigidbody;

    Vector3 moveDirection;
    Transform cameraObject;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity = 3;
    public float fallingVelocity = 33;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;


    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    public void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        cameraObject = Camera.main.transform;
        playerrigidbody = GetComponent<Rigidbody>();

    }


    public void HandleAllMovement()
    {

        if(playerManager.isInteracting)
            return;


        HandleMovement();
        HandleRotation();
    }


   private void HandleMovement()
    {
        if (isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
        if (inputManager.moveAmount > 0.5f)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
            
        }

        moveDirection = cameraObject.forward* inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;


        if(inputManager.moveAmount >= 0.5f)
        {
            moveDirection = moveDirection * runningSpeed;
        }
        else
        {
            moveDirection = moveDirection * walkingSpeed;
        }


        Vector3 movementVelocity = moveDirection;
        playerrigidbody.linearVelocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

        transform.rotation = targetRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y = raycastOrigin.y + rayCastHeightOffset;

        if (!isGrounded)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            playerrigidbody.AddForce(transform.forward * leapingVelocity);
            playerrigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);   
        
        }
            if(Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, 0.3f, groundLayer))
            {
                if(!isGrounded && playerManager.isInteracting)
                {
                    animatorManager.PlayTargetAnimation("Land", true);
                }
                inAirTimer = 0;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

    }



}