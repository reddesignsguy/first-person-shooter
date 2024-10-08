using System.Collections.Generic;
using UnityEngine;

public abstract class IAction
{
    public abstract void Execute(GameObject item1, GameObject item2);
}

public class ScoopAction : IAction
{
    public override void Execute(GameObject item1, GameObject item2)
    {
        if (item1.TryGetComponent(out Scooper scooper) && item2.TryGetComponent(out FoodSource foodSource))
        {
            // Scooper can only scoop if empty
            if (scooper.IsEmpty())
            {
                scooper.Fill(foodSource._ingredient);
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
    public override void Execute(GameObject item1, GameObject item2)
    {
        if (item1.TryGetComponent(out Scooper scooper) && item2.TryGetComponent(out IngredientReceiver receiver))
        {
            if (scooper.IsEmpty())
            {
                return;
            }

            // Pour all of the ingredient in scooper into the receiver
            receiver.Add(scooper._ingredient, scooper.capacity);
            scooper.MakeEmpty();
        }
        else
        {
            Debug.Log("Pour command was assigned to player incorrectly");
        }
    }
}

public class HoldAction : IAction
{

    public override void Execute(GameObject item1, GameObject item2)
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
        holdable.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void UpdateHeldItemState(GameObject item)
    {
        ItemContext.Instance._itemHeld = item;
    }
}