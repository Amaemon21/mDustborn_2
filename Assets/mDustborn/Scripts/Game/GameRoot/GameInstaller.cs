using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AssetSpriteLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<AssetConfigLoader>().AsSingle();
    }
}