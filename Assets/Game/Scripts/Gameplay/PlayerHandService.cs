using UnityEngine;
using Zenject;
using System;

using Game.Scripts.Gameplay.Views;
public class PlayerHandService : IDisposable
{
    private IInputService _inputService;
    private Transform _holdPoint;
    private CharacterView _characterView;
    private Camera _playerCamera;
    private float _interactionDistance = 10f;
    private HeavyObjectController _currentHeldObject;

    [SerializeField] private float _maxPossibleSize = 1.3f;
    [SerializeField] private float _throwForce = 10.0f;

    private Rigidbody _rb;
    private Collider _collider;
    private Vector2 _mouseDelta;

    public event Action<float> ObjectPickedUp;
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
        if (_currentHeldObject != null) return;

        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance))
        {
            if (hit.collider.TryGetComponent<HeavyObjectController>(out HeavyObjectController box))
            {
                    Vector3 boxSize = hit.collider.bounds.size;
                    float maxSide = Mathf.Max(boxSize.x, boxSize.y, boxSize.z);

                if (maxSide < _maxPossibleSize)
                {
                    _currentHeldObject = box;
                    _rb = _currentHeldObject.GetComponent<Rigidbody>();
                    _collider = _currentHeldObject.GetComponent<Collider>();
                    
                    _rb.isKinematic = true;
                    _collider.enabled = false;
                    _rb.useGravity = false;

                    _currentHeldObject.transform.SetParent(_holdPoint);
                    _currentHeldObject.transform.localPosition = Vector3.zero;
                    _currentHeldObject.transform.localRotation = Quaternion.identity;

                    float weight = _currentHeldObject.Weight;
                    ObjectPickedUp?.Invoke(weight);
                }
                else
                    Debug.Log("too big");
            }
        }
    }

    private void DropObject()
    {
        if (_currentHeldObject == null) return;

        _currentHeldObject.transform.SetParent(null);
        _rb.isKinematic = false;
        _collider.enabled = true;
        _rb.useGravity = true;

        _currentHeldObject = null;

        _mouseDelta = _inputService.MouseDelta;
        if(_mouseDelta.magnitude > 0.1f)
        {
            Vector3 throwDirection = _playerCamera.transform.forward;
            _rb.AddForce(throwDirection * _throwForce, ForceMode.Impulse);
        }

        ObjectDroppedDown?.Invoke();
    }
     
    public void Dispose()
    {
        _inputService.GetPressed -= PickUpObject;
        _inputService.DropPressed -= DropObject;
    }
}