using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private float _xRotation;

    public float xSensitivity = 80f;
    public float ySensitivity = 80f;


    public void ProcessLook (Vector2 direction)
    {
        // Rotate camera vertically]
        float mouseX = direction.x;
        float mouseY = direction.y;

        _xRotation -= (mouseY) * Time.deltaTime * xSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        Camera.main.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        // Rotate player horizontally
        float yTranslate = mouseX * Time.deltaTime * ySensitivity;
        transform.Rotate(new Vector3(0, yTranslate, 0));
    }
}
