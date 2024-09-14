using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private CharacterController _controller;
    private Vector3 _velocity;
    public float _speed = 5f;

    private bool _isGrounded = false;
    public float _gravity = -9.8f;
    public float _jumpHeight = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _isGrounded = _controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        // Apply movement
        Vector3 translation = new Vector3(input.x, 0, input.y);
        _controller.Move(transform.TransformDirection(translation) * _speed * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;

        // Apply gravity
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _controller.Move(_velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (_isGrounded)
        {   
            _velocity.y = Mathf.Sqrt(_jumpHeight  * (-3.0f) * _gravity);
        }
    }
}
