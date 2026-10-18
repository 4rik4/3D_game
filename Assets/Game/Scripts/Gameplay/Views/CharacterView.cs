using UnityEngine;

namespace Game.Scripts.Gameplay.Views
{
    public class CharacterView : MonoBehaviour
    {
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; set; } = 5.0f;
        [field: SerializeField] public float JumpSpeed { get; set; } = 5.0f;
        [field: SerializeField] public float Sensitivity { get; set; } = 3.0f;
    }
}