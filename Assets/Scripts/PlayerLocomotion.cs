using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Rigidbody playerrigidbody;

    Vector3 moveDirection;
    Transform cameraObject;

    public float movementSpeed = 7;
    public float rotationSpeed = 15;

    public void Awake()
    {
        inputManager = GetComponent<InputManager>();

        cameraObject = Camera.main.transform;
        playerrigidbody = GetComponent<Rigidbody>();

    }


    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }


   private void HandleMovement()
    {
        moveDirection = cameraObject.forward* inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;  


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


}
