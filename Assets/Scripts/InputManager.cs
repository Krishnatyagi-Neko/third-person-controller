using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    PlayerControls playercontrols;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;


    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool b_Input;
    public bool x_Input;
    public bool jump_Input;
    public bool dash_Input;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
    if(playercontrols == null)
    {
        playercontrols = new PlayerControls();
        playercontrols.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        playercontrols.PlayerMovement.Movement.canceled += i => movementInput = Vector2.zero;

        playercontrols.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        playercontrols.PlayerMovement.Camera.canceled += i => cameraInput = Vector2.zero;

        playercontrols.PlayerActions.B.performed += i => b_Input = true;
        playercontrols.PlayerActions.B.canceled += i => b_Input = false;
        playercontrols.PlayerActions.X.performed += i => x_Input = true;
        playercontrols.PlayerActions.Jump.performed += i => jump_Input = true;
        playercontrols.PlayerActions.Dash.performed += i => dash_Input = true;
        //playercontrols.PlayerActions.Jump.canceled += i => jump_Input = false;

    }
    playercontrols.Enable();
    }


    private void OnDisable()
    {
        playercontrols.Disable();
    }

    public void HandleAllInput()
    {
        HandleSprintingInput();
        HandleMovementInput();
        HandleJumpingInput();
        HandleDodgeInput();
        HandleDashInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;  
        cameraInputY = cameraInput.y;


        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0,moveAmount,playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if(b_Input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if(jump_Input)
        {
            jump_Input = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleDodgeInput()
    {
        if (x_Input)
        {
            x_Input = false;
            playerLocomotion.HandleDodge();
        }
    }

    private void HandleDashInput()
{
    if (dash_Input)
    {
        dash_Input = false;
        playerLocomotion.HandleDash();
    }
}

}