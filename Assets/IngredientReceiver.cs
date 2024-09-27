using System;
using System.Collections.Generic;

public class IngredientReceiver : Interactable
{
    public Dictionary<Ingredient, float> ingredientQuantities;

    private void Awake()
    {
        ingredientQuantities = new Dictionary<Ingredient, float>();
    }
}