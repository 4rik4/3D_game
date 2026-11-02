using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;

using Game.Scripts.Gameplay.Views;

public class PlayerHandService : IDisposable
{
    private IInputService _inputService;
    private Transform _holdPoint;
    private CharacterView _characterView;
    private Camera _playerCamera;

    private List<HeavyObjectController> _currentHeldObjects = new List<HeavyObjectController>();
    private float _stackHeight = 0f;
    public float stackWeight { get; private set; } = 0f;
    private float _currentBoxWeight;

    [SerializeField] private float _maxPossibleSize = 1.3f;
    [SerializeField] private float _throwForce = 10.0f;
    [SerializeField] private int _maxPossibleCount = 3;

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

        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, _characterView.InteractionDistance, _characterView.InteractableLayer) == false) return;

        if (hit.collider.TryGetComponent(out HeavyObjectController box) == false) return;

        Vector3 boxSize = hit.transform.localScale;
        float maxSide = Mathf.Max(boxSize.x, boxSize.y, boxSize.z);

        if (maxSide > _maxPossibleSize) {Debug.Log("too big"); return; }

        Rigidbody rb = box.GetComponent<Rigidbody>();
        Collider collider = box.GetComponent<Collider>();
        Debug.Log("sdfghjk");
        box.transform.SetParent(_holdPoint);
        rb.isKinematic = true;
        rb.useGravity = false;
        collider.enabled = false;

        float yOffset = _stackHeight + (boxSize.y / 2f);

        box.gameObject.layer = 1;
        box.transform.localPosition = new Vector3(0, yOffset, 0);
        box.transform.localRotation = Quaternion.identity;

        _currentHeldObjects.Add(box);

        _stackHeight += boxSize.y;

        _currentBoxWeight = box.Weight;
        stackWeight += _currentBoxWeight;
        ObjectPickedUp?.Invoke();
                    
    }

    private void DropObject()
    {
        if (_currentHeldObjects.Count == 0) return;

        HeavyObjectController boxToDrop = _currentHeldObjects[_currentHeldObjects.Count - 1];
        _currentHeldObjects.RemoveAt(_currentHeldObjects.Count - 1);

        boxToDrop.transform.SetParent(null);

        Rigidbody rb = boxToDrop.GetComponent<Rigidbody>();
        Collider collider = boxToDrop.GetComponent<Collider>();
        rb.isKinematic = false;
        collider.enabled = true;
        rb.useGravity = true;

        boxToDrop.gameObject.layer = boxToDrop.StartLayer;

        _mouseDelta = _inputService.MouseDelta;
        if(_mouseDelta.magnitude > 0.1f)
        {
            Vector3 throwDirection = _playerCamera.transform.forward;
            rb.AddForce(throwDirection * _throwForce, ForceMode.Impulse);
        }
        Debug.Log(boxToDrop.GetComponent<Collider>().bounds.size.y);

        _stackHeight -= boxToDrop.transform.localScale.y;
        Debug.Log(_stackHeight);
        stackWeight -= boxToDrop.Weight;
        ObjectDroppedDown?.Invoke();
    }
     
    public void Dispose()
    {
        _inputService.GetPressed -= PickUpObject;
        _inputService.DropPressed -= DropObject;
    }
}