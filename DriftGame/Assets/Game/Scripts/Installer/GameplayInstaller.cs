using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public UIGameplayManager uIGameplayManager;

    public override void InstallBindings()
    {
        Container.Bind<ITimerOnline>().To<Timer>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IOfflineTimer>().To<OfflineTimer>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IScoreController>().To<ScoreController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ICameraController>().To<GameplayCameraController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ICarFactory>().To<CarFactory>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IGameController>().To<GameController>().FromComponentInHierarchy().AsSingle();

        Container.BindInstance(uIGameplayManager).AsSingle();
    }
}
