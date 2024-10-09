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
    public List<IAction> actions { get; private set; } // TODO - replace with action data

    // TODO -- in the works
    List<ActionData> actionData;

    private void Awake()
    {
        actions = new List<IAction>();
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

    public void ExecuteAction(int actionIndex)
    {
        if (actions.Count <= actionIndex)
        {
            return;
        }

        GameObject itemHeld = ItemContext.Instance._itemHeld;
        GameObject itemLookingAt = ItemContext.Instance._itemLookingAt;
        actions[actionIndex].Execute(itemHeld, itemLookingAt);
    }

    /*
     * Returns a list of available actions the user can make given the item held and item being looked at inside the ItemContext.
     * ALL possible actions the player can make are to be given here. The actions available to the player is based on 
     * MANUALLY DEFINED component types that must be inherited by the items. This may cause an incorrect action to be given for a particular
     * pair of items in the context of gameplay design, however, because all actions check if the components needed are present in the ItemContext,
     * incorrect action assignments will not lead to run-time errors.
     */
    private List<IAction> GetAvailableActions()
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

        if (IsComponentPresentInItemLookingAt<Toggleable>())
        {
            commands.Add(new ToggleAction());
        }

        foreach (IAction command in commands)
            Debug.Log(command);

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

    private bool IsComponentPresentInItemLookingAt<T1>()
        where T1 : Component
    {
        GameObject itemLookingAt = ItemContext.Instance._itemLookingAt;

        return itemLookingAt != null && itemLookingAt.TryGetComponent(out T1 _);
    }

    private bool IsHandEmpty()
    {
        GameObject itemHeld = ItemContext.Instance._itemHeld;
        return itemHeld == null;
    }
}
