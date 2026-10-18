using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Views;
using UnityEngine;
using Zenject;

public class CharacterMover : ITickable
{
    private IInputService _input;
    private CharacterView _characterView;

    private float _maxSpeed;
    private float _jumpSpeed;
    private float _sensitivity;

    private Vector3 _velocity;
    private float _gravity = 9.81f;

    public CharacterMover(IInputService input, CharacterView characterView)
    {
        _input = input;
        _characterView = characterView;

        _maxSpeed = _characterView.MaxSpeed;
        _jumpSpeed = _characterView.JumpSpeed;
        _sensitivity = _characterView.Sensitivity;

        _input.JumpPressed += Jump;
    }

    private void Jump()
    {
        if (_characterView.CharacterController.isGrounded == false)
            return;

        _velocity.y = _jumpSpeed;
    }

    private void Move()
    {
        _characterView.CharacterController.Move(_velocity * Time.deltaTime);
    }

    private void Rotate()
    {

    }

    private void ChangeVelocity()
    {
        if (_characterView.CharacterController.isGrounded == false)
            _velocity.y -= _gravity * Time.deltaTime;

        Vector2 direction = _input.Direction;
        direction = _characterView.CharacterController.transform.TransformDirection(direction);

        _velocity.x = direction.x * _maxSpeed;
        _velocity.z = direction.y * _maxSpeed;
    }

    public void Tick()
    {
        ChangeVelocity();
        Move();
    }
}