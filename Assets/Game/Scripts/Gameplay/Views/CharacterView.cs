using UnityEngine;

namespace Game.Scripts.Gameplay.Views
{
    public class CharacterView : MonoBehaviour
    {
        /* [field: SerializeField] public CharacterController CharacterController { get; private set; }
         [field: SerializeField] public float MaxSpeed { get; private set; } = 5.0f;
         [field: SerializeField] public float SprintSpeed { get; private set; } = 100.0f;
         [field: SerializeField] public float JumpSpeed { get; private set; } = 5.0f;
         [field: SerializeField] public Transform Head { get; private set; } 
         [field: SerializeField] public float Sensetivity { get; private set; } = 10f;
         [field: SerializeField] public Camera PlayerCamera { get; private set; }
         [field: SerializeField] public float MaxVertAngle { get; private set; } = 75.0f;
         [field: SerializeField] public Transform HoldPoint { get; private set; }*/

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _maxSpeed = 5.0f;
        [SerializeField] private float _sprintSpeed = 100.0f;
        [SerializeField] private float _jumpSpeed = 5.0f;
        [SerializeField] private Transform _head;
        [SerializeField] private float _sensetivity = 10f;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _maxVertAngle = 75.0f;
        [SerializeField] private Transform _holdPoint;


        public CharacterController CharacterController => _characterController;
        public float MaxSpeed => _maxSpeed;
        public float SprintSpeed => _sprintSpeed;
        public float JumpSpeed => _jumpSpeed;
        public Transform Head => _head;
        public float Sensetivity => _sensetivity;
        public Camera PlayerCamera => _playerCamera;
        public float MaxVertAngle => _maxVertAngle;
        public Transform HoldPoint => _holdPoint;
    }
}