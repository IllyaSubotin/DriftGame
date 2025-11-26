using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayData : ICarData, ISettingsData, IOnlineService
{
    public CarSaveData[] carSaveDatas { get; set; }
    public int currentCarId { get; set; }

    public bool isOnlineMode{ get; set; }
    
    public float MusicVolume { get; set; } = 1f;
    public float SoundVolume { get; set; } = 1f;
}

[Serializable]
public class CarSaveData
{
    public bool isOwned;
    public int materialIndex;  
    public int wheelsIndex;     
    public bool spoilerEnabled; 
}