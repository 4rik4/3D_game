using UnityEngine;

namespace Game.Scripts.Gameplay.Views
{
    public class CharacterView : MonoBehaviour
    {
         [field: SerializeField] public CharacterController CharacterController { get; private set; }
         [field: SerializeField] public float MaxSpeed { get; private set; } = 5.0f;
         [field: SerializeField] public float SprintSpeed { get; private set; } = 100.0f;
         [field: SerializeField] public float JumpSpeed { get; private set; } = 5.0f;
         [field: SerializeField] public Transform Head { get; private set; } 
         [field: SerializeField] public float Sensetivity { get; private set; } = 10f;
         [field: SerializeField] public Camera PlayerCamera { get; private set; }
         [field: SerializeField] public float MaxVertAngle { get; private set; } = 75.0f;
         [field: SerializeField] public Transform HoldPoint { get; private set; }
         [field: SerializeField] public LayerMask InteractableLayer { get; private set; }
         [field: SerializeField] public float InteractionDistance { get; private set; } = 10f;
        [field: SerializeField] public int BoxCount { get; private set; } = 3;


    }
}