using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
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
        bool isSprinting = onFoot.Sprint.IsPressed();
        // Check if sprint is held
        motor.ProcessMove(moveDir, isSprinting);

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


public class CommandsManager
{
    // DI Held
    // DI LookingAt

    List<ICommand> commands;
    ItemContext _context;
    public CommandsManager()
    {
        _context = new ItemContext();
        CommandValidator validator = new CommandValidator(_context);

        _context._itemHeld = new Scooper();
        _context._itemLookingAt = new FoodSource();

        commands = new List<ICommand>();

        ScoopCommand scoop = new ScoopCommand(_context);
        commands.Add(scoop);

        HoldCommand hold = new HoldCommand(_context);
        commands.Add(hold);

    }

    public void GetCommands()
    {

        List<ICommand> validCommands = new List<ICommand>();

        foreach (ICommand command in commands)
        {
            if (CommandValidator.CommandIsValid(command))
            {
                validCommands.Add(command);
            }
            else
            {
                Console.WriteLine("FAILED TO ADD COMMAND: " + command);
            }
            Console.WriteLine("-------------------");
        }
        foreach (ICommand command in validCommands)
        {
            command.Execute();
        }
    }
}

public class CommandValidator
{

    public static ItemContext _context;

    public CommandValidator(ItemContext context)
    {
        _context = context;
    }

    public static bool CommandIsValid(ICommand command)
    {

        Type itemInHand = _context._itemHeld.GetType();
        Type itemLookAt = _context._itemLookingAt.GetType();
        Console.WriteLine(itemInHand);
        Console.WriteLine(itemLookAt);

        Type requiredHand = command._requirementsToExecute.TItemInHand;
        Type requiredLookAt = command._requirementsToExecute.TItemLookingAt;
        Console.WriteLine(requiredHand);
        Console.WriteLine(requiredLookAt);

        bool canUse = requiredHand.IsAssignableFrom(itemInHand) && requiredLookAt.IsAssignableFrom(itemLookAt);
        if (canUse)
        {
            return true;
        }

        Console.WriteLine("ERROR: Cannot use");
        return false;
    }
}

public class CommandRequirements
{

    public Type TItemInHand;
    public Type TItemLookingAt;

    public CommandRequirements(Type TItemInHand, Type TItemLookingAt)
    {
        this.TItemInHand = TItemInHand;
        this.TItemLookingAt = TItemLookingAt;
    }

}

public abstract class ICommand
{
    // Items required for this command
    public CommandRequirements _requirementsToExecute;
    private ItemContext _context; // Used explicitly for Execute behavior

    public ICommand(Type TItemInHand, Type TItemLookingAt, ItemContext _context)
    {
        this._context = _context;

        _requirementsToExecute = new CommandRequirements(TItemInHand, TItemLookingAt);
    }

    public void Execute()
    {

        // Extra layer of defense
        Console.WriteLine("Executing!");
        if (!CommandValidator.CommandIsValid(this))
        {
            return;
        }
        ExecutionBehaviorOverrideMe();
    }

    protected abstract void ExecutionBehaviorOverrideMe();
    //public virtual void Execute(); // In base: Do a verification check again with the item in hand  and item looking at
}

public class ScoopCommand : ICommand
{
    public ScoopCommand(ItemContext _context) : base(typeof(Scooper), typeof(FoodSource), _context)
    {
    }

    protected override void ExecutionBehaviorOverrideMe()
    {
        Console.WriteLine("scopoping!");
    }
}


public class HoldCommand : ICommand
{
    public HoldCommand(ItemContext _context) : base(typeof(EmptyItem), typeof(Item), _context)
    {
    }

    protected override void ExecutionBehaviorOverrideMe()
    {
        Console.WriteLine("holding!");
    }
}

public class Item
{

}

public class EmptyItem
{

}
public class Scooper : Item
{
    public float capacity;
}

public class FoodSource : Item
{

}

public class ItemContext
{
    public Item _itemHeld { get; set; }
    public Item _itemLookingAt { get; set; }
}