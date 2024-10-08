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
        string s = "";

        foreach ((Ingredient ingredient, float amount) in ingredients)
        {
            string unit = "";
            float numCups = amount / 16f;
            float finalAmount;

            switch (numCups)
            {
                case 1f:
                    unit = "cup";
                    finalAmount = numCups;
                    break;
                case >= 1f /  4f:
                    unit = "cups";
                    finalAmount = numCups;
                    break;
                case 1f / 16f:
                    unit = "tablespoon";
                    finalAmount = amount;
                    break;
                case >= 1f / 16f:
                    unit = "tablespoons";
                    finalAmount = amount;
                    break;
                default:
                    unit = "Unit not found";
                    finalAmount = amount;
                    break;
            }

            

            s += "<sprite name=\"" + ingredient + "\"> x " + finalAmount + " " + unit + "<br>";
        }

        print(s);
        text.text = s;
    }
}