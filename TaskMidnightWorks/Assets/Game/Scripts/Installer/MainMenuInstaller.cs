using Zenject;
using UnityEngine;
using System.Collections.Generic;

public class MainMenuInstaller : MonoInstaller
{
    public MainMenuScreen mainMenuScreen;
    public GarageScreen garageScreen;
    public ShopScreen shopScreen;
    public OnlineMenuScreen onlineMenuScreen;
    public DonateScreen donateScreen;
    public SettingScreen settingScreen;
    public MainMenuBootstrap bootstrap;

    public override void InstallBindings()
    {   
        Container.Bind<UIStateMachine>().AsSingle();

        Container.BindInstance(mainMenuScreen).AsSingle();
        Container.BindInstance(settingScreen).AsSingle();
        Container.BindInstance(garageScreen).AsSingle();
        Container.BindInstance(donateScreen).AsSingle();
        Container.BindInstance(shopScreen).AsSingle();
        Container.BindInstance(onlineMenuScreen).AsSingle();

        Container.Bind<MainMenuState>().AsSingle();
        Container.Bind<ShopState>().AsSingle();
        Container.Bind<GarageState>().AsSingle();
        Container.Bind<DonateState>().AsSingle();
        Container.Bind<SettingState>().AsSingle();
        Container.Bind<OnlineMenuState>().AsSingle();

        Container.BindInstance(bootstrap).AsSingle();

        

        
    }
}
