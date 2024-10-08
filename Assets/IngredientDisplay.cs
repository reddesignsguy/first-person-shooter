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

            // Determine
            string unit = "";
            float finalAmount;

            ConvertUnits(amount, out unit, out finalAmount);

            s += "<sprite name=\"" + ingredient + "\"> x " + finalAmount + " " + unit + "<br>";
        }

        text.text = s;
    }

    private void ConvertUnits(float amount, out string unit, out float finalAmount)
    {
        float numCups = amount / 16f;

        switch (numCups)
        {
            case 1f:
                unit = "cup";
                finalAmount = numCups;
                break;
            case >= 1f / 4f:
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
    }

    // Look at camera
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}