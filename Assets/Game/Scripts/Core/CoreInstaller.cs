using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CoreInstaller : MonoInstaller<CoreInstaller>
{
    public override void InstallBindings()
    {
        BindInputService();
    }

    private void BindInputService()
    {
        Container
            .BindInterfacesAndSelfTo<InputService>()
            .AsSingle()
            .NonLazy();
    }
}
