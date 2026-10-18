using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IInputService
{
    public Vector2 Direction { get; }
    public bool IsUsing { get; }
    public event Action JumpPressed;
    public event Action<Vector2> LookPerformed;
}

