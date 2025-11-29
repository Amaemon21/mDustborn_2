using Inventory;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private InventoryScreenView _screenView;
    
    public override void InstallBindings()
    {
        Container.Bind<WeaponsContainer>().AsSingle();
        Container.Bind<ScreenService>().AsSingle();

        Container.Bind<InventoryService>().AsSingle();
        
        Container.Bind<InventoryScreenViewModel>().AsSingle();
        
        Container.Bind<InventoryScreenView>().FromInstance(_screenView).AsSingle().NonLazy();
    }
}