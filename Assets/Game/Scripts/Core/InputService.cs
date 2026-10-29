using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputService : IInputService, IInitializable, IDisposable, ITickable
{
    private InputActions _input = new();

    private Vector2 _direction;
    private Vector2 _mouseDirection;
    private bool _isUsing;

    private float _resetDeltaTime = 6f;
    private float _idleTime = 0f;

    public event Action JumpPressed;
    public event Action<Vector2> LookPerformed;

    public event Action GetPressed;
    public event Action DropPressed;

    public Vector2 Direction => _direction;
    public Vector2 MouseDelta => _mouseDirection;
    public bool IsUsing => _isUsing;

    private void SubscribeToEvents()
    {
        _input.Enable();

        _input.Player.Walking.started += OnMove;
        _input.Player.Walking.performed += OnMove;
        _input.Player.Walking.canceled += OnMove;
        _input.Player.Jump.started += OnJump;
        _input.Player.Use.started += OnUse;
        _input.Player.Use.canceled += OnUse;
        _input.Player.Look.performed += Look;
        _input.Player.Look.canceled += StopLook;
        _input.Player.Take.started += OnGet;
        _input.Player.Drop.started += OnDrop;
    }

    private void OnGet(InputAction.CallbackContext context) 
    { 
        GetPressed?.Invoke();
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        DropPressed?.Invoke();
    }

    private void Look(InputAction.CallbackContext obj)
    {
        _mouseDirection = obj.ReadValue<Vector2>();
        LookPerformed?.Invoke(_mouseDirection);
        _idleTime = 0f;
    }

    private void StopLook(InputAction.CallbackContext context)
    {
        _mouseDirection = Vector2.zero;
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        _isUsing = context.ReadValueAsButton();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpPressed?.Invoke();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }

    private void UnsubscribeFromEvents()
    {
        _input.Player.Walking.started -= OnMove;
        _input.Player.Walking.performed -= OnMove;
        _input.Player.Walking.canceled -= OnMove;
        _input.Player.Jump.started -= OnJump;
        _input.Player.Use.started -= OnUse;
        _input.Player.Use.canceled -= OnUse;
        _input.Player.Look.performed -= Look;
        _input.Player.Look.canceled -= StopLook;
        _input.Player.Take.started -= OnGet;
        _input.Player.Drop.started -= OnDrop;

        _input.Disable();
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();
    }

    public void Initialize()
    {
        SubscribeToEvents();
    }

    public void Tick()
    {
        if(_mouseDirection != Vector2.zero)
        {
            _idleTime += Time.deltaTime;

            if( _idleTime > _resetDeltaTime )
            {
                _mouseDirection = Vector2.zero;
                _idleTime = 0;
            }
        }
    }
}