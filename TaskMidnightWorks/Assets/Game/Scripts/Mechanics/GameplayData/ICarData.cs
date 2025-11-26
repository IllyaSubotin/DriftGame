using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarData
{
    CarSaveData[] carSaveDatas { get; set; }
    public int currentCarId { get; set; }
}
