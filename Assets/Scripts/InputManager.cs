using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor.Timeline.Actions;
using UnityEngine;

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

    List<IAction> validCommands;
    ActionManager commandGetter;

    private void Start()
    {
        Cursor.visible = false;

        commandGetter = new ActionManager();

        validCommands = commandGetter.GetAvailableActions();

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
            IAction command = validCommands.Count > 0 ? validCommands[0] : null;
            command?.Execute(ItemContext.Instance._itemHeld, ItemContext.Instance._itemLookingAt);
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


public class Interactable : MonoBehaviour
{
    
}

public enum Ingredient
{
    None,
    Flour,
    Sugar
}