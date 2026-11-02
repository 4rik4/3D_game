using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

using Game.Scripts.Gameplay.Views;

public class BoxDeliveryService 
{
    private TriggerView _triggerView;
    private CharacterView _characterView;
    public event Action OnWon;
    public event Action<HeavyObjectController> OnDestroy;
    private int _boxesDelivered = 0;
    private int _boxCount;

    public BoxDeliveryService(TriggerView triggerView, CharacterView characterView)
    {
        _triggerView = triggerView;
        _characterView = characterView;

        _boxCount = _characterView.BoxCount;
        _triggerView.TriggerEntered += OnDelivered;
    }

    private void OnDelivered(Collider other)
    {
        if (other.TryGetComponent(out HeavyObjectController box) == false) return;
        Debug.Log("dostavleno");
        if (!box.IsDelivering) return;
        OnDestroy?.Invoke(box);

        _boxesDelivered++;
        if (_boxesDelivered == _boxCount)
        {
            Debug.Log("victory");
            OnWon?.Invoke();
        }
    }
}
