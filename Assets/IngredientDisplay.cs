using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class IngredientDisplay : MonoBehaviour
{
    TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();    
    }

    public void UpdateText(Dictionary<Ingredient, float> ingredients)
    {
        text.text = "testing from script";
        string s = "";

        foreach ((Ingredient ingredient, float amount) in ingredients)
        {

            s += "<sprite=" + ingredient + "> x " + amount + " cup<br>";
        }

        print(s);
        text.text = s;
    }
}