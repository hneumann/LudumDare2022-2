using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private GameController gameController;
    [SerializeField] private BumblebeeController bumblebeeController;
    [SerializeField] private ShopController shopController;
    
    public override void InstallBindings()
    {
        Container.Bind<GameController>().FromInstance(gameController).AsSingle().NonLazy();
        Container.Bind<BumblebeeController>().FromInstance(bumblebeeController).AsSingle().NonLazy();
        Container.Bind<ShopController>().FromInstance(shopController).AsSingle().NonLazy();
    }
}