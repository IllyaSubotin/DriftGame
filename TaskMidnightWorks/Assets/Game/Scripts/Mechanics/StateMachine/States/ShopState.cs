using System.Collections;
using Zenject;
using UnityEngine;
using Unity.VisualScripting;

public class ShopState : UIState
{
    private int _currentCarId = 0;
    private bool _isSetup = false;

    private IMoneyManager _moneyManager;
    private UIStateMachine _uIStateMachine;
    private SaveLoadManager _saveLoadManager;
    private ICarData _carData;
    private ShopScreen _screen;

    [Inject]
    public void Construct(IMoneyManager moneyManager, UIStateMachine uIStateMachine, ShopScreen shopScreen, ICarData carData, SaveLoadManager saveLoadManager)
    {
        _moneyManager = moneyManager;
        _carData = carData;
        _saveLoadManager = saveLoadManager;
        _uIStateMachine = uIStateMachine;
        _screen = shopScreen;
    }

    public override void Enter()
    {
        _screen.Show();
        foreach(var car in _carData.carSaveDatas)
        {
            if (car.isOwned)
            {
                _currentCarId++;
            }
        }
        

        SetCurrentCarId(_currentCarId);
        SetupButton();
    }

    public override void Exit()
    {
        ResetAllCarPosition();
        _screen.Hide();
    }

    private void SetupButton()
    {
        if (_isSetup) return;

        _isSetup = true;

        _screen.previosCarButton.onClick.AddListener(() =>
        {
            SetCurrentCarId(_currentCarId - 1);
        });

        _screen.nextCarButton.onClick.AddListener(() =>
        {
            SetCurrentCarId(_currentCarId + 1);
        });

        _screen.cashBuyButton.onClick.AddListener(() =>
        {
            if (_moneyManager.Get(MoneyType.Cash) >= _screen.carsPriceConfig.CashPrice[_currentCarId])
            {
                _moneyManager.Spend(_screen.carsPriceConfig.CashPrice[_currentCarId], MoneyType.Cash);
                _carData.carSaveDatas[_currentCarId].isOwned = true;
                _saveLoadManager.SaveGameData();
            }
        });

        _screen.gemsBuyButton.onClick.AddListener(() =>
        {
            if (_moneyManager.Get(MoneyType.Gems) >= _screen.carsPriceConfig.GemPrice[_currentCarId])
            {
                _moneyManager.Spend(_screen.carsPriceConfig.GemPrice[_currentCarId], MoneyType.Gems);
                _carData.carSaveDatas[_currentCarId].isOwned = true;
                _saveLoadManager.SaveGameData();
            }
        });

        _screen.menuButton.onClick.AddListener(() =>
        {
            _uIStateMachine.ChangeState<MainMenuState>();
        });
    }

    private void SetCurrentCarId(int newId)
    {
        _screen.carsController[_currentCarId].gameObject.SetActive(true);
        _screen.carsController[_currentCarId].MoveToPoint(_screen.carPositionPonts[2].position);

        _currentCarId = newId;
        //_carData.currentCarId = _currentCarId;

        if (_currentCarId == 0)
            _screen.previosCarButton.interactable = false;
        else
            _screen.previosCarButton.interactable = true;

        if (_currentCarId == 4)
            _screen.nextCarButton.interactable = false;
        else
            _screen.nextCarButton.interactable = true;

        UpdatePriceText();
        ActivateCar();
    }
    
    
    private void UpdatePriceText()
    {
        if (_currentCarId == 0 || _carData.carSaveDatas[_currentCarId].isOwned)
        {
            _screen.cashBuyButton.gameObject.SetActive(false);
            _screen.gemsBuyButton.gameObject.SetActive(false);
        }
        else if (!_carData.carSaveDatas[_currentCarId - 1].isOwned)
        {
            _screen.cashBuyButton.gameObject.SetActive(false);
            _screen.gemsBuyButton.gameObject.SetActive(false);

            _screen.carNotAvailableText.gameObject.SetActive(true);
        }
        else
        {
            _screen.cashBuyButton.gameObject.SetActive(true);
            _screen.gemsBuyButton.gameObject.SetActive(true);
            _screen.cashBuyText.text = _screen.carsPriceConfig.CashPrice[_currentCarId].ToString();
            _screen.gemsBuyTexts.text = _screen.carsPriceConfig.GemPrice[_currentCarId].ToString();

            _screen.carNotAvailableText.gameObject.SetActive(false);
        }
        

    }


    private void ActivateCar()
    {
        _screen.cars[_currentCarId].SetActive(true);
        _screen.carsController[_currentCarId].SetPosition(_screen.carPositionPonts[0].position);
        _screen.carsController[_currentCarId].MoveToPoint(_screen.carPositionPonts[1].position);

    }

    private void ResetAllCarPosition()
    {
        foreach(var car in _screen.carsController)
        {
            car.gameObject.SetActive(true);
            car.SetPosition(_screen.carPositionPonts[0].position);
            car.gameObject.SetActive(false);
        }
    }

}
