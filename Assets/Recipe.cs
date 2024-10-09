using System;
using System.Collections.Generic;
using UnityEngine;

//public class Recipe
//{
//    Dictionary<Ingredient, float> ingredients;

//    public Recipe()
//    {
//        ingredients = new Dictionary<Ingredient, float>();
//    }

//    public Recipe Add(Ingredient i, float amt)
//    {
//        ingredients[i] = amt;
//        return this;
//    }
//}

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [Header("Ingredients")]
    public List<Ingredient> ingredientList;
    public List<float> amounts;

    public Dictionary<Ingredient, float> ToDictionary()
    {
        Dictionary<Ingredient, float> ingredientDictionary = new Dictionary<Ingredient, float>();

        // Check if both lists have the same count
        if (ingredientList.Count != amounts.Count)
        {
            Debug.LogError("Ingredient list and amounts list must have the same count.");
            return ingredientDictionary; // Return an empty dictionary in case of error
        }

        // Populate the dictionary
        for (int i = 0; i < ingredientList.Count; i++)
        {
            ingredientDictionary[ingredientList[i]] = amounts[i];
        }

        return ingredientDictionary;
    }
}