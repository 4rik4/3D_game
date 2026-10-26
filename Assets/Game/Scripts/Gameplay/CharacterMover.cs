using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Views;
using UnityEngine;
using Zenject;

public class CharacterMover : ITickable
{
    private IInputService _input;
    private CharacterView _characterView;
    private HeavyObjectController _boxView;
    private PlayerHandService _handService;
    private Camera _playerCamera;

    private float _maxSpeed;
    private float _jumpSpeed;
    private float _sensetivity;
    private float _maxVertAngle;
    private float _weight;

    private Vector3 _velocity;
    private float _gravity = 9.81f;
    private float _currentVertAngle;

    private float _bobSpeed = 10f;
    private Vector3 _bobAmount = new Vector3(0f, 0.1f, 0f);
    private Vector3 _cameraOriginalPos;
    private float _bobTimer = 0f;

    public CharacterMover(IInputService input, CharacterView characterView, PlayerHandService handService)
    {
        _input = input;
        _characterView = characterView;
        _handService = handService;

        _maxVertAngle = _characterView.MaxVertAngle;
        _maxSpeed = _characterView.MaxSpeed;
        _jumpSpeed = _characterView.JumpSpeed;
        _sensetivity = _characterView.Sensetivity;


        _playerCamera = _characterView.PlayerCamera;

        _input.JumpPressed += Jump;
        _input.LookPerformed += Rotate;
        _handService.ObjectPickedUp += PickUp;
        _handService.ObjectDroppedDown += DropDown;


        Cursor.lockState = CursorLockMode.Locked;
        _cameraOriginalPos = _playerCamera.transform.localPosition;
    }

    private void PickUp(float boxWeight)
    {
        _maxSpeed -= boxWeight * 0.4f;
        _jumpSpeed -= boxWeight / 3f;
    }

    private void DropDown()
    {
        _maxSpeed = _characterView.MaxSpeed;
        _jumpSpeed = _characterView.JumpSpeed;
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
        HeadBob();
    }

    private void Rotate(Vector2 input)
    {
        float rotationY = input.x * _sensetivity * Time.deltaTime;
        _characterView.transform.rotation *= Quaternion.Euler(0f, rotationY, 0f);

        float rotationX = input.y * _sensetivity * Time.deltaTime;
        _characterView.Head.transform.rotation *= Quaternion.Euler(rotationX, rotationY, 0f);
        _currentVertAngle -= rotationX;
        _currentVertAngle = Mathf.Clamp(_currentVertAngle, -_maxVertAngle, _maxVertAngle);
        _characterView.Head.localEulerAngles = new Vector3(_currentVertAngle, 0f, 0f);
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
    }

    public void Tick()
    {
        ChangeVelocity();
        Move();
    }
}