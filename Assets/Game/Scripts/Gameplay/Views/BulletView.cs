using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Gameplay.Views
{
    public class BulletView : MonoBehaviour
    {
        private Bullet _bullet;
        [field: SerializeField] public float Damage { get; private set; } = 10;
        [field: SerializeField] public float LifeTime { get; private set; } = 10;
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public float Speed { get; private set; } = 10f;

        private void Awake()
        {
            _bullet = new Bullet(this);
        }
    }
}
