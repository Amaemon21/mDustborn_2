using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<GameStatePlayerPrefsProvider>().AsSingle();
        
        Container.Bind<AssetConfigLoader>().AsSingle();
        Container.Bind<AssetSpriteLoader>().AsSingle();
    }
}