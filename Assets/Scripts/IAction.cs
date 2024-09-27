using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public void Execute(GameObject item1, GameObject item2);
}

public class ScoopAction : IAction
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
        }
        else
        {
            Debug.Log("Scoop command was assigned to player incorrectly");
        }
    }

}

public class PourAction : IAction
{
    public void Execute(GameObject item1, GameObject item2)
    {
        if (item1.TryGetComponent(out Scooper scooper) && item2.TryGetComponent(out IngredientReceiver receiver))
        {
            if (scooper._ingredient == Ingredient.None)
            {
                Debug.Log("Trying to pour nothing");
                return;
            }    

            Debug.Log("Pouring");
            // Pour all of the ingredient in scooper into the receiver
            UpdateReceiver(scooper._ingredient, scooper._capacity, receiver);
            UpdateScooper(scooper);
        }
        else
        {
            Debug.Log("Pour command was assigned to player incorrectly");
        }
    }

    private void UpdateScooper(Scooper scooper)
    {
        scooper._ingredient = Ingredient.None;
    }

    private void UpdateReceiver(Ingredient i, float amount, IngredientReceiver receiver)
    {
        Dictionary<Ingredient, float> ingredientQuantities = receiver.ingredientQuantities;
        ingredientQuantities[i] = ingredientQuantities.ContainsKey(i) ? ingredientQuantities[i] + amount : amount;
    }
}

public class HoldAction : IAction
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
        }
        else
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