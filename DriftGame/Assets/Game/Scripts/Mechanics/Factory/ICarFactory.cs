using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarFactory 
{
    GameObject SpawnPlayerCar(Transform spawnPoint);
    GameObject SpawnPlayerCarOnline(Transform spawnPoint);
}
