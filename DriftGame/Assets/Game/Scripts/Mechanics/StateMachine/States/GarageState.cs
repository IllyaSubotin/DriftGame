using System.Collections;
using Zenject;
using UnityEngine;
using System;
using System.Linq;

public class GarageState : UIState
{
    public event Action<int> onSelected;

    private int _currentCarId;
    private bool _isSetup = false;
    private const int VARIANT_COUNT = 5; 

    private GarageScreen _screen;
    private SaveLoadManager _saveLoadManager;
    private UIStateMachine _uIStateMachine;
    private DiContainer _container;    
    private ICarData _carData;

    [Inject]
    public void Construct(GarageScreen garageScreen, SaveLoadManager saveLoadManager, DiContainer container, ICarData carData, UIStateMachine uIStateMachine)
    {
        _screen = garageScreen;
        _saveLoadManager = saveLoadManager;
        _uIStateMachine = uIStateMachine;
        _container = container;
        _carData = carData;
    }

    public override void Enter()
    {
        _screen.Show();
        _currentCarId = _carData.currentCarId;
        
        SetupButton();
        ActivateCar();
    }

    public override void Exit()
    {
        _saveLoadManager.SaveGameData();
        ResetAllCarPosition();
        _screen.Hide();
    }

    private void SetupButton()
    {
        if (_isSetup) return;

        _isSetup = true;
        
        var count = 0;
        foreach(var car in _carData.carSaveDatas)
            if (car.isOwned) 
                count++;   
        

        _screen.spoilerButton.interactable = _screen.carsController[_currentCarId].IsSpoiler();
        
        _screen.carButton.onClick.AddListener(() =>
        {
            onSelected = SetCurrentCarId;
            ShowVariants(_screen.variantTitleTexts[0], _screen.variantTexts[0], count, onSelected);
        });

        _screen.colorButton.onClick.AddListener(() =>
        {
            onSelected = SetMaterial;
            ShowVariants(_screen.variantTitleTexts[1], _screen.variantTexts[1], VARIANT_COUNT, onSelected);
        });

        _screen.wheelButton.onClick.AddListener(() =>
        {
            onSelected = SetWheels;
            ShowVariants(_screen.variantTitleTexts[2], _screen.variantTexts[2], VARIANT_COUNT, onSelected);
        });

        _screen.spoilerButton.onClick.AddListener(() =>
        {
            var spoilerEnabled = _screen.carsController[_currentCarId].EnableSpoiler();
            _carData.carSaveDatas[_currentCarId].spoilerEnabled = spoilerEnabled;
        });

        _screen.menuButton.onClick.AddListener(() =>
        {
            _uIStateMachine.ChangeState<MainMenuState>();
        });
    }


    private void ShowVariants(string title, string variantText, int count, Action<int> OnSelected)
    {
        _screen.variantPanel.SetActive(false);
        _screen.variantPanel.SetActive(true);
        _screen.variantText.text = title;

        for (int i = 0; i < count; i++)
        {
            var go = _container.InstantiatePrefab(_screen.variantPrefab, _screen.variantButtonsPanel.transform);
            var variantInfo = go.GetComponent<VariantPanel>();

            variantInfo.variantName.text = $"{variantText} {i + 1}";

            int index = i;
            variantInfo.variantButton.onClick.AddListener(() =>
            {
                onSelected?.Invoke(index);
                _screen.variantPanel.SetActive(false);
            });
        }
    }

    private void SetCurrentCarId(int newId)
    {
        if (_currentCarId == newId)
            return;

        _screen.carsController[_currentCarId].gameObject.SetActive(true);
        _screen.carsController[_currentCarId].MoveToPoint(_screen.carPositionPonts[2].position);

        _currentCarId = newId;
        _carData.currentCarId = _currentCarId;

        _screen.spoilerButton.interactable = _screen.carsController[_currentCarId].IsSpoiler();
        ActivateCar();
    }

    private void SetMaterial(int id)
    {
        _screen.carsController[_currentCarId]
                .SetMaterial(_screen.carDataConfigs[_currentCarId]
                    .materials[id]);

        _carData.carSaveDatas[_currentCarId].materialIndex = id;
    }

    private void SetWheels(int id)
    {
        _screen.carsController[_currentCarId].SetWheels(id);

        _carData.carSaveDatas[_currentCarId].wheelsIndex = id;
    }

    private void ActivateCar()
    {
        _screen.cars[_currentCarId].SetActive(true);
        _screen.carsController[_currentCarId].SetPosition(_screen.carPositionPonts[0].position);
        _screen.carsController[_currentCarId].MoveToPoint(_screen.carPositionPonts[1].position);
    }

    private void ResetAllCarPosition()
    {
        foreach (var car in _screen.carsController)
        {
            car.gameObject.SetActive(true);
            car.SetPosition(_screen.carPositionPonts[0].position);
            car.gameObject.SetActive(false);
        }
    }
    
    
}
