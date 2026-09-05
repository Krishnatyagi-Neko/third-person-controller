using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playercontrols;

    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;


    private void OnEnable()
    {
        if(playercontrols == null)
        {
            playercontrols = new PlayerControls();
            playercontrols.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

    playercontrols.Enable();

    }


    private void OnDisable()
    {
        playercontrols.Disable();
    }

    public void HandleAllInput()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }


}
