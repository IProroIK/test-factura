using Core.Controllers;
using Core.Service;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CarController _carController;
    
    public override void InstallBindings()
    {
        Container.Bind<IPoolService>().To<PoolService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LevelController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CameraController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CarController>()
            .FromInstance(_carController)
            .AsSingle()
            .NonLazy();
        Container.BindInterfacesAndSelfTo<GroundController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<EnemysController>().AsSingle().NonLazy();
    }
}

