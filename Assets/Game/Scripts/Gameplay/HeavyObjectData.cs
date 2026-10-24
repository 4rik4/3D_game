using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyObjectData
{
    public Transform Transform { get; }
    public Rigidbody Rigidbody { get; }
    public Collider Collider { get; }

    public HeavyObjectData(Transform transform, Rigidbody rigidbody, Collider collider)
    {
        Transform = transform;
        Rigidbody = rigidbody;
        Collider = collider;
    }
}
