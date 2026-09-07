using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    PlayerLocomotion playerLocomotion;
    InputManager inputManager;
    CameraManager cameraManager;

    private void Awake()
    {
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
    }

}
    