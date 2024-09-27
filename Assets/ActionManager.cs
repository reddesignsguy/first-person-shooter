using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private GameObject lastItemHeld;
    private GameObject lastItemLookingAt;
    public List<IAction> actions { get; private set; }

    private void Update()
    {
        // Null check
        if (!lastItemHeld || !lastItemLookingAt)
        {
            lastItemHeld = ItemContext.Instance._itemHeld;
            lastItemLookingAt = ItemContext.Instance._itemLookingAt;
        }

        bool itemContextChanged = lastItemHeld != ItemContext.Instance._itemHeld || lastItemLookingAt != ItemContext.Instance._itemLookingAt;
        if (itemContextChanged)
        {
            actions = GetAvailableActions();
        }
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
