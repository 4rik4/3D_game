using UnityEngine;

namespace Game.Scripts.Gameplay.Views
{
    public class CharacterView : MonoBehaviour
    {
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; set; } = 5.0f;
        [field: SerializeField] public float JumpSpeed { get; set; } = 5.0f;
        [field: SerializeField] public float Sensitivity { get; set; } = 0.01f;
        [field: SerializeField] public Camera PlayerCamera { get; private set; }
        [field: SerializeField] public float MaxLookAngle { get; private set; } = 50.0f;
        [field: SerializeField] public Transform HoldPoint { get; private set; }

    }
}