using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Views;
using UnityEngine;
using Zenject;

public class CharacterMover : ITickable
{
    private IInputService _input;
    private CharacterView _characterView;
    private Camera _playerCamera;

    private float _maxSpeed;
    private float _jumpSpeed;

    private Vector3 _velocity;
    private float _gravity = 9.81f;

    private float _maxLookAngle;
    private float _sensivity;
    private float _pitch = 0.0f;
    private Vector3 _lookInput;

    public CharacterMover(IInputService input, CharacterView characterView)
    {
        _input = input;
        _characterView = characterView;

        _maxSpeed = _characterView.MaxSpeed;
        _jumpSpeed = _characterView.JumpSpeed;
        _sensivity = _characterView.Sensitivity;

        _playerCamera = _characterView.PlayerCamera;
        _maxLookAngle = _characterView.MaxLookAngle;

        _input.JumpPressed += Jump;
        _input.LookPerformed += OnLook;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnLook(Vector2 lookDelta)
    {
        _lookInput = lookDelta;
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
        Vector2 look = _lookInput * _sensivity * Time.deltaTime;

        _characterView.CharacterController.transform.Rotate(Vector3.up * look.x);

        _pitch -= look.y;
        _pitch = Mathf.Clamp(_pitch, -_maxLookAngle, _maxLookAngle);

        _playerCamera.transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

        _lookInput = Vector2.zero;
    }

    private void ChangeVelocity()
    {
        if (_characterView.CharacterController.isGrounded == false)
            _velocity.y -= _gravity * Time.deltaTime;


        Vector2 direction = _input.Direction;

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        moveDirection = _characterView.CharacterController.transform.TransformDirection(moveDirection);

        _velocity.x = moveDirection.x * _maxSpeed;
        _velocity.z = moveDirection.z * _maxSpeed;
    }

    public void Tick()
    {
        ChangeVelocity();
        Move();
        Rotate();
    }
}