using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IInputService
{
    public Vector2 MouseDelta { get; }
    public Vector2 Direction { get; }
    public bool IsUsing { get; }
    public bool IsSprinting { get; }
    public event Action ShiftStarted;
    public event Action ShiftStopped;
    public event Action JumpPressed;
    public event Action<Vector2> LookPerformed;
    public event Action GetPressed;
    public event Action DropPressed;
}

