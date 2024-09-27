using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scooper : Interactable
{
    public float _capacity;
    public Ingredient _ingredient;

    public Ingredient Pour()
    {
        Ingredient temp = _ingredient;
        temp = Ingredient.None;

        return temp;
    }

    public bool IngredientIsAccepted(Ingredient i)
    {
        return _ingredient == i;
    }

}
