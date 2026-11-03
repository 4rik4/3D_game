using UnityEngine;
using System;

namespace Game.Scripts.Gameplay.Views
{
    //zapisat zapros Rb && collider
    public class BoxView : MonoBehaviour
    {
        [field: SerializeField] public float Weight { get; private set; }
        [field: SerializeField] public bool IsDelivering { get; private set; } = false;

        public Rigidbody RB { get; private set; }
        public Collider Coll { get; private set; }

        public int StartLayer { get; private set; }

        public event Action<BoxView, GlassWallView, float> BoxCollised;

        private void Awake()
        {
            StartLayer = gameObject.layer;

            RB = GetComponent<Rigidbody>();
            Coll = GetComponent<Collider>();
        }

        public void BoxDelivered()
        {
            gameObject.layer = 0;

            RB.useGravity = false;
            RB.isKinematic = true;
        }

        private void OnCollisionEnter(Collision collision)//вообще сомнительная тема, так вообще в зенджекте можно?
        {
            if (collision.collider.TryGetComponent(out GlassWallView wall) == false) return;

            float speed = collision.relativeVelocity.magnitude;

            BoxCollised?.Invoke(this, wall, speed);
        }
    }
}

