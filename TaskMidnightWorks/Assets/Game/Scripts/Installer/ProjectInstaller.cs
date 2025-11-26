using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public LevelPlayAdsManager levelPlayAdsManager;
    public override void InstallBindings()
    {
        Container.Bind<IMultiplayerManager>().To<MultiplayerManager>().FromComponentInHierarchy().AsSingle();

        Container.Bind<GameplayData>().AsSingle().NonLazy();
        Container.Bind<LevelPlayAdsManager>().FromComponentInNewPrefab(levelPlayAdsManager).AsSingle().NonLazy();
        Container.Bind<ICarData>().FromResolveGetter<GameplayData>(x => x).AsSingle();
        Container.Bind<ISettingsData>().FromResolveGetter<GameplayData>(x => x).AsSingle();
        Container.Bind<IOnlineService>().FromResolveGetter<GameplayData>(x => x).AsSingle();
        
        Container.Bind<MoneyData>().AsSingle();
        Container.BindInterfacesTo<MoneyManager>().AsSingle();

        Container.Bind<SaveLoadManager>().AsSingle().NonLazy();
        Container.Bind<SaveLoadService>().AsSingle().NonLazy();
    }
}