using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
