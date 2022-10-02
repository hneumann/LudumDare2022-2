using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private GameController gameController;
    [SerializeField] private BumblebeeController bumblebeeController;
    [SerializeField] private InvisibleWalls invisibleWalls;
    [SerializeField] private ShopController shopController;
    [SerializeField] private GravityController gravityController;
    
    public override void InstallBindings()
    {
        Container.Bind<GameController>().FromInstance(gameController).AsSingle().NonLazy();
        Container.Bind<BumblebeeController>().FromInstance(bumblebeeController).AsSingle().NonLazy();
        Container.Bind<InvisibleWalls>().FromInstance(invisibleWalls).AsSingle().NonLazy();
        Container.Bind<ShopController>().FromInstance(shopController).AsSingle().NonLazy();
        Container.Bind<GravityController>().FromInstance(gravityController).AsSingle().NonLazy();
    }
}