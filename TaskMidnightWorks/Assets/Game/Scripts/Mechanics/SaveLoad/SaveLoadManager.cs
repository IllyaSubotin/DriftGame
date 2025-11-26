using UnityEngine;
using Zenject;

public class SaveLoadManager
{
    private GameplayData _gameplayData;
    private SaveLoadService _saveLoad;
    private IMoneyManager _moneyManager;
    [Inject]
    private void Construct(GameplayData gameplayData, IMoneyManager moneyManager, SaveLoadService saveLoad)
    {
        _gameplayData = gameplayData;
        _moneyManager = moneyManager;
        _saveLoad = saveLoad;
    }


    public void LoadGameData()
    {
        var data = _saveLoad.Load();
        ApplyToGameplayData(data);
    }

    private void ApplyToGameplayData(SaveData data)
    {
        if (data.carSaveDatas != null)
        {
            _gameplayData.MusicVolume = data.Volume;
            _gameplayData.SoundVolume = data.Sensitivity;

            _moneyManager.Add(data.cash, MoneyType.Cash);
            _moneyManager.Add(data.gems, MoneyType.Gems);

            _gameplayData.carSaveDatas = data.carSaveDatas;
            _gameplayData.currentCarId = data.currentCarId;

        }
        else
        {
            _gameplayData.MusicVolume = 0;
            _gameplayData.SoundVolume = 0;

            _moneyManager.Add(0, MoneyType.Cash);
            _moneyManager.Add(0, MoneyType.Gems);

            CarSaveData[] carsSave = new CarSaveData[5];

            for (int i = 0; i < 5; i++)
            {
                carsSave[i] = new CarSaveData();
                carsSave[i].isOwned = false;
                carsSave[i].materialIndex = 0;
                carsSave[i].wheelsIndex = 0;
                carsSave[i].spoilerEnabled = false;
            }

            carsSave[0].isOwned = true;

            _gameplayData.carSaveDatas = carsSave;
            _gameplayData.currentCarId = 0;
        }
    }
    


    public void SaveGameData()
    {
        var data = ConvertToSaveData();
        _saveLoad.Save(data);
    }

    private SaveData ConvertToSaveData()
    {
        return new SaveData
        {
            Volume = _gameplayData.MusicVolume,
            Sensitivity = _gameplayData.SoundVolume,

            cash = _moneyManager.Get(MoneyType.Cash),
            gems = _moneyManager.Get(MoneyType.Gems),

            carSaveDatas = _gameplayData.carSaveDatas,
            currentCarId = _gameplayData.currentCarId,

        };
    }
}
