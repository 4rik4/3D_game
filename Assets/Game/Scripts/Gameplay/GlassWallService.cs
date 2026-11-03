using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Game.Scripts.Gameplay.Views;

public class GlassWallService : IDisposable
{
    private PlayerHandService _handService;
    private BoxView _box;

    private float _impulse;

    public GlassWallService(PlayerHandService handService)
    {
        _handService = handService;

        _handService.ObjectDroppedDown += OnWallCollised;
    }

    public void Dispose()
    {
        if (_handService != null) _handService.ObjectDroppedDown -= OnWallCollised;
    }

    private void OnWallCollised(BoxView box)
    {
        box.BoxCollised += OnWallBroken;
    }

    private void OnWallBroken(BoxView box, GlassWallView wall, float speed)
    {
        _impulse = speed * box.Weight;

        if (_impulse < wall.Strength) return;
        wall.gameObject.SetActive(false);
    }
}
