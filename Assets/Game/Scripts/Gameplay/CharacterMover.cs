using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Views;
using UnityEngine;
using Zenject;

public class CharacterMover : ITickable
{
    private IInputService _input;
    private CharacterView _characterView;
    private PlayerHandService _handService;
    private Camera _playerCamera;

    private float _maxSpeed;
    private float _jumpSpeed;

    private Vector3 _velocity;
    private float _gravity = 9.81f;

    private float _maxLookAngle;
    private float _pitch = 0.0f;
    private Vector3 _lookInput;

    private float _bobSpeed = 10f;
    private Vector3 _bobAmount = new Vector3(0f, 0.1f, 0f);
    private Vector3 _cameraOriginalPos;
    private float _bobTimer = 0f;

    public CharacterMover(IInputService input, CharacterView characterView, PlayerHandService handService)
    {
        _input = input;
        _characterView = characterView;
        _handService = handService;

        _maxSpeed = _characterView.MaxSpeed;
        _jumpSpeed = _characterView.JumpSpeed;

        _playerCamera = _characterView.PlayerCamera;
        _maxLookAngle = _characterView.MaxLookAngle;

        _input.JumpPressed += Jump;
        _input.LookPerformed += OnLook;
        _handService.ObjectPickedUp += PickUp;
        _handService.ObjectDroppedDown += DropDown;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _cameraOriginalPos = _playerCamera.transform.localPosition;
    }

    private void PickUp()
    {
        _maxSpeed -= 4;
        _jumpSpeed -= 2.5f;
    }

    private void DropDown()
    {
        _maxSpeed = _characterView.MaxSpeed;
        _jumpSpeed = _characterView.JumpSpeed;
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
        Vector2 look = _lookInput * _characterView.Sensitivity * 0.01f;

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

    private void HeadBob()
    {
        Vector3 horizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        if (currentSpeed > 0.01f)
        {
            float speedMultiplier = currentSpeed / _maxSpeed;
            _bobTimer += Time.deltaTime * _bobSpeed * speedMultiplier;

            _playerCamera.transform.localPosition = new Vector3(0, _cameraOriginalPos.y + Mathf.Sin(_bobTimer) * _bobAmount.y, 0);
        }
        else
            _bobTimer = Mathf.Lerp(_bobTimer, 0f, Time.deltaTime * _bobSpeed);
        /*bool isWalking = _input.Direction.sqrMagnitude > 0.01f && _characterView.CharacterController.isGrounded;

        if (isWalking)
        {
            _bobTimer += Time.deltaTime * _bobSpeed;

            _playerCamera.transform.localPosition = new Vector3(0, _cameraOriginalPos.y + Mathf.Sin(_bobTimer) * _bobAmount.y, 0);
        }
        else
        {
            _bobTimer = 0f;
            _playerCamera.transform.localPosition = Vector3.Lerp(
                _playerCamera.transform.localPosition,
                _cameraOriginalPos,
                Time.deltaTime * _bobSpeed
            );
        }*/
    }

    public void Tick()
    {
        ChangeVelocity();
        Move();
        Rotate();
        HeadBob();
    }
}