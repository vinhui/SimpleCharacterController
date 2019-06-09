using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerInput : MonoBehaviour, GameInput.IPlayerActions
{
    public GameInput gameInput;
    public CharacterControllerMotor motor;

    private void Awake()
    {
        gameInput = new GameInput();
        gameInput.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        gameInput.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        gameInput.Player.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled)
        {
            Vector2 axis = context.ReadValue<Vector2>();
            motor.DoMove(axis.x, axis.y);
        }
        else
        {
            motor.StopMove();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                motor.IsSprinting = true;
                break;
            case InputActionPhase.Canceled:
                motor.IsSprinting = false;
                break;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 axis = context.ReadValue<Vector2>();
        motor.DoRotate(axis.x, -axis.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        motor.DoJump();
    }
}