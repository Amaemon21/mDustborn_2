using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        
        Container.Bind<WeaponsContainer>().AsSingle();
        Container.Bind<ScreenService>().AsSingle();

        Container.Bind<GameStatePlayerPrefsProvider>().AsSingle();
        
        //Container.BindInterfacesAndSelfTo<InteractHandler>().AsSingle();
    }
}