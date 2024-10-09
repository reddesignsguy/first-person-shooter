using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private ActionManager actionManager;
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
        actionManager = GetComponent<ActionManager>();

        onFoot.Jump.performed += ctx => motor.Jump();
    }

    private void Start()
    {
        Cursor.visible = false;

    }

    private void Update()
    {
        Vector2 moveDir = onFoot.Movement.ReadValue<Vector2>();
        bool isSprinting = onFoot.Sprint.IsPressed();
        // Check if sprint is held
        motor.ProcessMove(moveDir, isSprinting);

        Vector2 lookDir = onFoot.Look.ReadValue<Vector2>();
        _look.ProcessLook(lookDir);

        if (Input.GetKey(KeyCode.E))
        {
            actionManager.ExecuteAction(0);
        }
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


public abstract class Interactable : MonoBehaviour
{
    
}

public enum Ingredient
{
    None,
    Flour,
    Sugar
}