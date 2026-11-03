using System;
using UnityEngine;

namespace Game.Scripts.Gameplay.Views
{
    public class TriggerView : MonoBehaviour
    {
        public event Action<BoxView> BoxEntered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BoxView box))
                BoxEntered?.Invoke(box);
        }
    }
}

