using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientReceiver : Interactable
{
    public Dictionary<Ingredient, float> ingredientQuantities;

    private void Awake()
    {
        ingredientQuantities = new Dictionary<Ingredient, float>();
    }

    public void Add (Ingredient i, float amount)
    {
        ingredientQuantities[i] = ingredientQuantities.ContainsKey(i) ? ingredientQuantities[i] + amount : amount;

        foreach (var kvp in ingredientQuantities)
        {
            Debug.Log($"Ingredient: {kvp.Key}, Quantity: {kvp.Value}");
        }

    }
}