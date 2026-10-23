using UnityEngine;
using Zenject;
using System;

public class PlayerHandService : IInitializable, IDisposable
{
    private IInputService _inputService;
    private Transform _holdPoint;
    private Camera _playerCamera;
    private float _interactionDistance = 3.5f;
    private LayerMask _heavyLayer;

    private HeavyObjectData _currentHeldObject;

    public event Action ObjectPickedUp;
    public event Action ObjectDroppedDown;

    public PlayerHandService(
        IInputService inputService,
        Transform holdPoint,
        Camera playerCamera,
        LayerMask heavyLayer)
    {
        _inputService = inputService;
        _holdPoint = holdPoint;
        _playerCamera = playerCamera;
        _heavyLayer = heavyLayer;
    }

    public void Initialize()
    {
        _inputService.GetPressed += PickUpObject;
        _inputService.DropPressed += DropObject;
    }

    private void PickUpObject()
    {
        if (_currentHeldObject != null) return;

        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, _heavyLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                _currentHeldObject = new HeavyObjectData(hit.transform, rb);

                _currentHeldObject.Rigidbody.isKinematic = true;
                _currentHeldObject.Rigidbody.useGravity = false;

                _currentHeldObject.Transform.SetParent(_holdPoint);
                _currentHeldObject.Transform.localPosition = Vector3.zero;
                _currentHeldObject.Transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void DropObject()
    {
        if (_currentHeldObject == null) return;

        _currentHeldObject.Transform.SetParent(null);
        _currentHeldObject.Rigidbody.isKinematic = false;
        _currentHeldObject.Rigidbody.useGravity = true;

        _currentHeldObject = null;
    }

    public void Dispose()
    {
        _inputService.GetPressed -= PickUpObject;
        _inputService.DropPressed -= DropObject;
    }
}