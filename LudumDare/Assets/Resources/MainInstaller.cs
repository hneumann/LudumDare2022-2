using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private GameController gameController;
    [SerializeField] private BumblebeeController bumblebeeController;
    [SerializeField] private InvisibleWalls invisibleWalls;
    
    public override void InstallBindings()
    {
        Container.Bind<GameController>().FromInstance(gameController).AsSingle().NonLazy();
        Container.Bind<BumblebeeController>().FromInstance(bumblebeeController).AsSingle().NonLazy();
        Container.Bind<InvisibleWalls>().FromInstance(invisibleWalls).AsSingle().NonLazy();
    }
}