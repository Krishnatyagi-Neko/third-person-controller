using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    PlayerLocomotion playerLocomotion;
    InputManager inputManager;
    CameraManager cameraManager;


    public bool isInteracting;
    public bool isUsingRootMotion;

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
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
    
        cameraManager.HandleAllCameraMovement();

      isInteracting = animator.GetBool("isInteracting");
      playerLocomotion.isJumping = animator.GetBool("isJumping");
      isUsingRootMotion = animator.GetBool("isUsingRootMotion");
      if (!playerLocomotion.isDashing)
        {
            animator.SetBool("isGrounded", playerLocomotion.isGrounded);
        }

    }

}
    