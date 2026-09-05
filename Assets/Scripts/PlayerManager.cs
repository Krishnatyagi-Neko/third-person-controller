using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    PlayerLocomotion playerLocomotion;
    InputManager inputManager;

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
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


}
    