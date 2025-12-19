using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainMenuState : UIState
{
    private UIStateMachine _stateMachine;
    private MainMenuScreen _screen;
    private SaveLoadManager _saveLoadManager;
    private IMoneyManager _moneyManager;
    private IOnlineService _onlineService;
    private IMultiplayerManager _multiplayerManager;

    [Inject]
    public void Construct(UIStateMachine stateMachine, MainMenuScreen mainMenuScreen,
                            SaveLoadManager saveLoadManager, IMoneyManager moneyManager,
                                IOnlineService onlineService, IMultiplayerManager multiplayerManager)
    {
        _stateMachine = stateMachine;
        _screen = mainMenuScreen;
        _onlineService = onlineService;
        _multiplayerManager = multiplayerManager;
        _saveLoadManager = saveLoadManager;
        _moneyManager = moneyManager;
    }

    public override void Enter()
    {
        _screen.Show();
        SetupButtons();
        _screen.cashText.text = _moneyManager.Get(MoneyType.Cash).ToString();
        _screen.gemsText.text = _moneyManager.Get(MoneyType.Gems).ToString();
        _multiplayerManager.Disconnect();

    }




    public override void Exit()
    {
        //_screen.Hide();
    }
    
    private void SetupButtons()
    {
        _screen.singlePlayButton.onClick.AddListener(() =>
        {
            _onlineService.isOnlineMode = false;
            SceneManager.LoadScene("FirstMap");
        });

        _screen.multiPlayButton.onClick.AddListener(() =>
        {
            _onlineService.isOnlineMode = true;
            _stateMachine.ChangeState<OnlineMenuState>();
        });

        _screen.garageButton.onClick.AddListener(() =>
        {
            _screen.Hide();
            _stateMachine.ChangeState<GarageState>();
        });

        _screen.carShopButton.onClick.AddListener(() =>
        {
            _screen.Hide();
            _stateMachine.ChangeState<ShopState>();
        });

        _screen.exitButton.onClick.AddListener(() =>
        {
            _saveLoadManager.SaveGameData();
            
            Application.Quit();
        });
    }
}
