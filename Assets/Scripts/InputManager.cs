using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerMotor motor;
    private PlayerLook _look;
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent <PlayerMotor>();
        _look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump();
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 moveDir = onFoot.Movement.ReadValue<Vector2>();
        motor.ProcessMove(moveDir);

        Vector2 lookDir = onFoot.Look.ReadValue<Vector2>();
        _look.ProcessLook(lookDir);

    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
