using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCarPrefabs", menuName = "Scriptable Objects/CarPrefabs")]
public class CarPrefabsConfig : ScriptableObject
{
    public GameObject[] carPrefabs;
    public String[] carPrefabsName;
}
