using System.Collections;
using System.Collections.Generic;
using System;
using Zenject;
using UnityEngine;

using Game.Scripts.Gameplay.Views;

public class BoxDeliveryService : IDisposable
{
    private TriggerView _triggerView;
    private CharacterView _characterView;

    public event Action OnWon;
  //  public event Action<BoxView> OnDestroy;

    private int _boxesDelivered = 0;
    private int _boxCount;

    public BoxDeliveryService(TriggerView triggerView, CharacterView characterView)
    {
        _triggerView = triggerView;
        _characterView = characterView;

        _boxCount = _characterView.BoxCount;

        _triggerView.BoxEntered += OnDelivered;
    }

    private void OnDelivered(BoxView box)
    {
        if (!box.IsDelivering) return;
        box.BoxDelivered();
  //      OnDestroy?.Invoke(box);

        _boxesDelivered++;

        if (_boxesDelivered == _boxCount)
        {
            Debug.Log("victory");
            OnWon?.Invoke();
        }
    }

    public void Dispose()
    {
        _triggerView.BoxEntered -= OnDelivered;
    }
}
