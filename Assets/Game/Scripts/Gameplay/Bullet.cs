using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Views;
using UnityEngine;

public class Bullet 
{
    private BulletView _bulletView;

    public Bullet(BulletView bulletView)
    {
        _bulletView = bulletView;
        _bulletView.Rigidbody.AddForce(_bulletView.transform.forward * _bulletView.Speed, ForceMode.Impulse);
    }
    
    
}
