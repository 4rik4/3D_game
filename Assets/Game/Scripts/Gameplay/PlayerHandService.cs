using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Views;

public class PlayerHandService : IDisposable
{
    private IInputService _inputService;
    private CharacterView _characterView;

    private Transform _holdPoint;
    private Camera _playerCamera;

    private List<HeavyObjectController> _currentHeldObjects = new List<HeavyObjectController>();

    private float _stackHeight = 0f;
    public float stackWeight { get; private set; } = 0f;
    
    private float _maxPossibleSize = 1.3f;
    private float _throwForce = 10.0f;
    private int _maxPossibleCount = 3;
    private float _maxPossibleWeight = 10f;

    private Vector2 _mouseDelta;

    public event Action ObjectPickedUp;
    public event Action ObjectDroppedDown;

    public PlayerHandService(IInputService inputService, CharacterView view)
    {
        _inputService = inputService;
        _characterView = view;

        _holdPoint = _characterView.HoldPoint;
        _playerCamera = _characterView.PlayerCamera;

        _inputService.GetPressed += PickUpObject;
        _inputService.DropPressed += DropObject;
    }

    private void PickUpObject()
    {
        if (_currentHeldObjects.Count == _maxPossibleCount) {Debug.Log("низзя брат, слишком многа");  return; }
        if (_maxPossibleWeight < stackWeight){ Debug.Log("а не надорвешься?");  return; }

        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, _characterView.InteractionDistance, _characterView.InteractableLayer) == false) return;

        if (hit.collider.TryGetComponent(out HeavyObjectController box) == false) return;

        Vector3 boxSize = hit.transform.localScale;
        float maxSide = Mathf.Max(boxSize.x, boxSize.y, boxSize.z);

        if (maxSide > _maxPossibleSize) {Debug.Log("too big"); return; }

        box.RB.isKinematic = true;
        box.RB.useGravity = false;
        box.Coll.enabled = false;

        box.transform.SetParent(_holdPoint);

        float yOffset = _stackHeight + (boxSize.y / 2f);
        box.transform.localPosition = new Vector3(0, yOffset, 0);
        box.transform.localRotation = Quaternion.identity;

        box.gameObject.layer = 1;

        _currentHeldObjects.Add(box);

        _stackHeight += boxSize.y;
        stackWeight += box.Weight;

        ObjectPickedUp?.Invoke();
    }

    private void DropObject()
    {
        if (_currentHeldObjects.Count == 0) return;

        int iLast = _currentHeldObjects.Count - 1;
        HeavyObjectController boxToDrop = _currentHeldObjects[iLast];
        _currentHeldObjects.RemoveAt(iLast);

        boxToDrop.transform.SetParent(null);

        boxToDrop.RB.isKinematic = false;
        boxToDrop.RB.useGravity = true;
        boxToDrop.Coll.enabled = true;

        boxToDrop.gameObject.layer = boxToDrop.StartLayer;

        _mouseDelta = _inputService.MouseDelta;
        if(_mouseDelta.magnitude > 0.1f)
        {
            Vector3 throwDirection = _playerCamera.transform.forward;
            boxToDrop.RB.AddForce(throwDirection * _throwForce, ForceMode.Impulse);
        }

        _stackHeight -= boxToDrop.transform.localScale.y;
        stackWeight -= boxToDrop.Weight;

        ObjectDroppedDown?.Invoke();
    }
     
    public void Dispose()
    {
        _inputService.GetPressed -= PickUpObject;
        _inputService.DropPressed -= DropObject;
    }
}