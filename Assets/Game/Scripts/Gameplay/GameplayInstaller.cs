using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Views;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller<GameplayInstaller>
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private CharacterView _characterPrefab;
    private Transform _holdPoint;

    public override void InstallBindings()
    {
        BindCharacterView();
        BindCharacterMover();
        BindHoldPoint();
    }

    private void BindCharacterView()
    {
        Container
            .Bind<CharacterView>()
            .FromComponentInNewPrefab(_characterPrefab)
            .UnderTransform(_spawnPoint)
            .AsSingle()
            .NonLazy();
    }

    private void BindCharacterMover()
    {
        Container
            .BindInterfacesAndSelfTo<CharacterMover>()
            .AsSingle()
            .NonLazy();
    }

    private void BindHoldPoint()
    {
        CharacterView character = Container.Resolve<CharacterView>();

        _holdPoint = character.HoldPoint;

        Container
        .BindInterfacesAndSelfTo<PlayerHandService>()
        .AsSingle()
        .WithArguments(_holdPoint, character.PlayerCamera, (LayerMask)LayerMask.GetMask("Heavy"));
    }

}