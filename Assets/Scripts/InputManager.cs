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

public class ActionManager
{
    public List<IAction> GetAvailableActions()
    {
        List<IAction> commands = new List<IAction>();

        if (AreComponentsPresent<Scooper, FoodSource>())
        {
            commands.Add(new ScoopAction());
        }

        if (AreComponentsPresent<Scooper, IngredientReceiver>())
        {
            commands.Add(new PourAction());
        }

        if (IsHandEmpty() && IsComponentPresentInItemLookingAt<Holdable>())
        {
            commands.Add(new HoldAction());
        }
        
        return commands;
    }

    bool AreComponentsPresent<T1, T2>()
        where T1 : Interactable
        where T2 : Interactable
    {
        GameObject itemHeld = ItemContext.Instance._itemHeld;
        GameObject itemLookingAt = ItemContext.Instance._itemLookingAt;

        return itemHeld != null && itemHeld.TryGetComponent(out T1 _) && itemLookingAt != null && itemLookingAt.TryGetComponent(out T2 _);
    }

    bool IsComponentPresentInItemLookingAt<T1>()
        where T1 : Component
    {
        GameObject itemLookingAt = ItemContext.Instance._itemLookingAt;

        return itemLookingAt != null && itemLookingAt.TryGetComponent(out T1 _);
    }

    bool IsHandEmpty()
    {
        GameObject itemHeld = ItemContext.Instance._itemHeld;
        return itemHeld == null;
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