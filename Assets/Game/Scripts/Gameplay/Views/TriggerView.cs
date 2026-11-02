using System;
using UnityEngine;
using Game.Scripts.Gameplay.Views;

public class TriggerView : MonoBehaviour
{
    public event Action<HeavyObjectController> BoxEntered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out HeavyObjectController box))
            BoxEntered?.Invoke(box);
    }

}

