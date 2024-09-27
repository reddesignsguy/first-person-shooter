using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scooper : Interactable
{
    [SerializeField]
    private float _capacity;

    public float capacity
    {
        get { return _capacity; }
        private set { _capacity = value; } // Private setter prevents modifications from outside
    }

    public Ingredient _ingredient { get; private set; }

    public bool IsEmpty()
    {
        return _ingredient == Ingredient.None;
    }

    public void MakeEmpty()
    {
        _ingredient = Ingredient.None;
    }

    public void Fill(Ingredient i)
    {
        if (i == Ingredient.None)
        {
            return;
        }

        _ingredient = i;
    }
}
