using Game.Scripts.Gameplay.Views;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Gameplay
{
    public class TurretShooter : ITickable
    {
        private CharacterView _characterView;
        private TurretView _turretView;
        private bool _inTrigger = false;
        private Transform _target = null;
        
        public TurretShooter(TurretView turretView)
        {
            _turretView = turretView;
            _turretView.RangeTriggerEntered += Triggered;
            _turretView.RangeTriggerExited += Exited;
            
        }

        private void Exited(Collider obj)
        {
            _target = null;
        }

        private void Triggered(Collider obj)
        {
            if (obj.TryGetComponent(out _characterView))
            {
                _inTrigger = true;
                _target = obj.transform;
            }
        }

        public void Tick()
        {
            if (!_inTrigger) return;
            _turretView.transform.rotation = Quaternion.LookRotation(_target.position - _turretView.transform.position);
            
        }

        private void OnShoot()
        {
            
        }
    }
}