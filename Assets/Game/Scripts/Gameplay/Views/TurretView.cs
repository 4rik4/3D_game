using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Gameplay.Views
{
    public class TurretView : MonoBehaviour
    {
        [field: SerializeField] public float Range { get; private set; } = 15f;
        [field: SerializeField] public float RoationSpeed { get; private set; } = 1f;
        [field: SerializeField] public float ShootForce  { get; private set; } = 10f;
        [field: SerializeField] public float CooldownTime  { get; private set; } = 0.5f;
        [field: SerializeField] public BulletView Bullet {get; private set;}
        
        private TurretShooter _turretShooter;

        public event Action<Collider> RangeTriggerEntered; 
        public event Action<Collider> RangeTriggerExited; 
        
        private void Awake()
        {
            _turretShooter = new TurretShooter(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            RangeTriggerEntered?.Invoke(other);
        }
        private void OnTriggerExit(Collider other)
        {
            RangeTriggerExited?.Invoke(other);
        }
    }
}
