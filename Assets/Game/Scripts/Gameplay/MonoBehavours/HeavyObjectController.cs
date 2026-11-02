using UnityEngine;

namespace Game.Scripts.Gameplay.Views
{
    //zapisat zapros Rb && collider
    public class HeavyObjectController : MonoBehaviour
    {
        [field: SerializeField] public float Weight { get; private set; }
        public int StartLayer { get; private set; }

        private void Awake()
        {
            StartLayer = gameObject.layer;
        }
    }   
}
