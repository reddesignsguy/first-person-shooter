using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ActionData
{
    public IAction Action { get; private set; }
    public ScriptableObject ActionSuggestion { get; private set; }

    // Constructor
    public ActionData(IAction action, ScriptableObject actionSuggestion)
    {
        Action = action;
        ActionSuggestion = actionSuggestion;
    }
}

public class ActionManager : MonoBehaviour
{

    private GameObject lastItemHeld;
    private GameObject lastItemLookingAt;
    public Dictionary<KeyCode, IAction> actions { get; private set; } // TODO - replace with action data

    // TODO -- in the works
    List<ActionData> actionData;

    private void Awake()
    {
        actions = new Dictionary<KeyCode, IAction>();
        actionData = new List<ActionData>();
    }

    private void Update()
    {
        bool itemContextChanged = lastItemHeld != ItemContext.Instance._itemHeld || lastItemLookingAt != ItemContext.Instance._itemLookingAt;
        if (itemContextChanged)
        {
            actions = GetAvailableActions();
        }

        lastItemHeld = ItemContext.Instance._itemHeld;
        lastItemLookingAt = ItemContext.Instance._itemLookingAt;
    }

    public void ExecuteAction(KeyCode code)
    {
        if (actions.TryGetValue(code, out IAction action))
        {
            GameObject itemHeld = ItemContext.Instance._itemHeld;
            GameObject itemLookingAt = ItemContext.Instance._itemLookingAt;
            action.Execute(itemHeld, itemLookingAt);
            }
    }

    /*
     * Returns a list of available actions the user can make given the item held and item being looked at inside the ItemContext.
     * ALL possible actions the player can make are to be given here. The actions available to the player is based on 
     * MANUALLY DEFINED component types that must be inherited by the items. This may cause an incorrect action to be given for a particular
     * pair of items in the context of gameplay design, however, because all actions check if the components needed are present in the ItemContext,
     * incorrect action assignments will not lead to run-time errors.
     */
    private Dictionary<KeyCode, IAction> GetAvailableActions()
    {
        //List<IAction> commands = new List<IAction>();

        Dictionary<KeyCode, IAction> commands = new Dictionary<KeyCode, IAction>();
        KeyCode keyCode;

        // TODO -- refactor
        // All actions that can be assigned to E
        keyCode = KeyCode.E;
        if (IsHolding<Scooper>())
        {
            if (IsLookingAt<FoodSource>())
            {
                commands[keyCode] = new ScoopAction();
            }
            else if (IsLookingAt<IngredientReceiver>())
            {
                commands[keyCode] = new PourAction();
            }
        }
        else if (IsHandEmpty() && IsLookingAt<Holdable>())
        {
            commands[keyCode] = new HoldAction();
        }

        // All actions that can be assigned to left mouse button
        keyCode = KeyCode.Mouse0;
        if (IsLookingAt<Toggleable>())
        {
            commands[keyCode] = new ToggleAction(); 
        }

        foreach(var pair in commands)
        {
            print(pair.Key);
            print(pair.Value);
        }

        return commands;
    }

    private bool AreComponentsPresent<T1, T2>()
        where T1 : Interactable
        where T2 : Interactable
    {
        GameObject itemHeld = ItemContext.Instance._itemHeld;
        GameObject itemLookingAt = ItemContext.Instance._itemLookingAt;

        return itemHeld != null && itemHeld.TryGetComponent(out T1 _) && itemLookingAt != null && itemLookingAt.TryGetComponent(out T2 _);
    }

    private bool IsLookingAt<T1>()
        where T1 : Component
    {
        GameObject itemLookingAt = ItemContext.Instance._itemLookingAt;

        return itemLookingAt != null && itemLookingAt.TryGetComponent(out T1 _);
    }

    private bool IsHolding<T1>()
    where T1 : Component
    {
        GameObject itemHeld = ItemContext.Instance._itemHeld;

        return itemHeld != null && itemHeld.TryGetComponent(out T1 _);
    }


    private bool IsHandEmpty()
    {
        GameObject itemHeld = ItemContext.Instance._itemHeld;
        return itemHeld == null;
    }
}
