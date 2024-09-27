using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

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

    List<ICommand> commands; // TODO -- testing. Move to a singleton
    List<ICommand> validCommands;
    GameObject sphere;
    GameObject heldItem;
    CommandGetter commandGetter;

    private void Start()
    {
        Cursor.visible = false;

        commandGetter = new CommandGetter();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerItemHolder holder = player.GetComponent<PlayerItemHolder>();

        ItemContext.Instance._itemLookingAt = GameObject.Find("Sphere");

        validCommands = commandGetter.GetAvailableActions();

    }

    private void Update()
    {
        validCommands = commandGetter.GetAvailableActions();
        foreach (ICommand command in validCommands)
            Debug.Log(command);

        Vector2 moveDir = onFoot.Movement.ReadValue<Vector2>();
        bool isSprinting = onFoot.Sprint.IsPressed();
        // Check if sprint is held
        motor.ProcessMove(moveDir, isSprinting);

        Vector2 lookDir = onFoot.Look.ReadValue<Vector2>();
        _look.ProcessLook(lookDir);

        if (Input.GetKey(KeyCode.E))
        {
            ICommand command = validCommands.Count > 0 ? validCommands[0] : null;
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

public class CommandGetter
{

    public List<ICommand> GetAvailableActions()
    {
        List<ICommand> commands = new List<ICommand>();

        if (AreComponentsPresent<Scooper, FoodSource>())
        {
            commands.Add(new ScoopCommand());
        }

        if (IsHandEmpty() && IsComponentPresentInItemLookingAt<Holdable>())
        {
            commands.Add(new HoldCommand());
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

public interface ICommand
{
    public void Execute(GameObject item1, GameObject item2);
}

public class ScoopCommand : ICommand
{
    public void Execute(GameObject item1, GameObject item2)
    {
        if (item1.TryGetComponent(out Scooper scooper) && item2.TryGetComponent(out FoodSource foodSource))
        {
            // Scooper can only scoop if empty
            if (scooper._ingredient == Ingredient.None)
            {
                scooper._ingredient = foodSource._ingredient;
            }
        } else
        {
            Debug.Log("Scoop command was assigned to player incorrectly");
        }
    }
}

public class HoldCommand : ICommand
{

    public void Execute(GameObject item1, GameObject item2)
    {
        if (item1 == null && item2.TryGetComponent(out Holdable holdable))
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerItemHolder holder = player?.GetComponent<PlayerItemHolder>();
            if (holder == null)
            {
                return;
            }

            AttachToPlayerHolder(holdable, holder);
            UpdateHeldItemState(item2);
        } else
        {
            Debug.Log("Hold command was assigned to player incorrectly");
        }
    }

    private void AttachToPlayerHolder(Holdable holdable, PlayerItemHolder holder)
    {
        holdable.transform.parent = holder.equipPosition;
        holdable.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void UpdateHeldItemState(GameObject item)
    {
        ItemContext.Instance._itemHeld = item;
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


public class Manager
{
    void Main()
    {
        List<ICommand> commands = new List<ICommand>();
        foreach (ICommand command in commands)
        {

        }
    }
}