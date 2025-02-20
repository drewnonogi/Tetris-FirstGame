using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnBlockRotationAction;
    public event EventHandler OnMoveDownActionActive;
    public event EventHandler OnMoveDownActionCanceled;
    public event EventHandler OnMoveLeftAction;
    public event EventHandler OnMoveRightAction;


    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.BlockRotation.performed += BlockRotation_performed;
        playerInputActions.Player.MoveDown.performed += MoveDown_performed;
        playerInputActions.Player.MoveDown.canceled += MoveDown_canceled;
        playerInputActions.Player.MoveLeft.performed += MoveLeft_performed;
        playerInputActions.Player.MoveRight.performed += MoveRight_performed;
    }
    private void OnDestroy()
    {
        playerInputActions.Player.BlockRotation.performed -= BlockRotation_performed;
        playerInputActions.Player.MoveDown.performed -= MoveDown_performed;
        playerInputActions.Player.MoveDown.canceled -= MoveDown_canceled;
        playerInputActions.Player.MoveLeft.performed -= MoveLeft_performed;
        playerInputActions.Player.MoveRight.performed -= MoveRight_performed;
        playerInputActions.Player.Disable();
        playerInputActions.Dispose();
    }

    private void MoveDown_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMoveDownActionCanceled(this, EventArgs.Empty);
    }

    private void MoveRight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMoveRightAction?.Invoke(this, EventArgs.Empty);
    }

    private void MoveLeft_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMoveLeftAction?.Invoke(this, EventArgs.Empty);

    }

    private void MoveDown_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMoveDownActionActive?.Invoke(this, EventArgs.Empty);

    }

    private void BlockRotation_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnBlockRotationAction?.Invoke(this, EventArgs.Empty);
    }
}
