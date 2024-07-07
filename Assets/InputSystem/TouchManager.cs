using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPress;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPress;
    }

    private void TouchPress(InputAction.CallbackContext context)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(touchPositionAction.
            ReadValue<Vector2>());
    }
}
