using System;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
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
    public bool isJumping;


    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;


    [Header("Jumping Speeds")]
    public float jumpHeight = 3;
    public float GravityIntensity = -15;

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
        HandleFallingAndLanding();

         if(isJumping)
            return;

        if(playerManager.isInteracting)
            return;


        HandleMovement();
        HandleRotation();
    }

   private void HandleMovement()
    {
        if(isJumping)
            return;

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
        if(isJumping)
        return;

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
    Vector3 targetPosition;
    targetPosition = transform.position;


    if (!isGrounded && !isJumping)
    {
        if (!playerManager.isInteracting)
        {
            animatorManager.PlayTargetAnimation("Falling", true);
        }
        inAirTimer = inAirTimer + Time.deltaTime;
        playerrigidbody.AddForce(transform.forward * leapingVelocity);
        playerrigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);   
    }

    if(Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, 0.6f, groundLayer))
    {
        if(!isGrounded && playerManager.isInteracting)
        {
            animatorManager.PlayTargetAnimation("Land", true);
        }

        Vector3 rayCastHitPoint = hit.point;
        targetPosition.y = rayCastHitPoint.y;
        inAirTimer = 0;
        isGrounded = true;
    }
    else
    {
        isGrounded = false;
    }

    if(isGrounded && !isJumping)
        {
            if(playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

}

    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jumping", false); // <- match the actual state name

            float jumpingVelocity = Mathf.Sqrt(-2 * GravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerrigidbody.linearVelocity = playerVelocity;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + Vector3.up * rayCastHeightOffset;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(origin + Vector3.down * 0.3f, 0.2f);
    }


}