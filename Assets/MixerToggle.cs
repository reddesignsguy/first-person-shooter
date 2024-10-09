using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MixerToggle : Toggleable
{

    private GameObject result;

    // TODO refactor
    public Recipe requirement;

    public override void Toggle()
    {
        if (transform.TryGetComponent(out IngredientReceiver receiver))
        {
            foreach(var pair in receiver.ingredientQuantities)
            {
                Ingredient ingredient = pair.Key;
                float amount = pair.Value;

                
            }
        }

        // Get Ingredient Receiver component
        // Check ingredients
        // If ingredients are correct, then output GOOD dough
        // else: output BAD dough
    }
}
