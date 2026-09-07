using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    PlayerLocomotion playerLocomotion;
    InputManager inputManager;
    CameraManager cameraManager;


    public bool isInteracting;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraManager = FindObjectOfType<CameraManager>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInput();
        playerLocomotion.HandleAllMovement();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

      isInteracting = animator.GetBool("isInteracting");

    }

}
    