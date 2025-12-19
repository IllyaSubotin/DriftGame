using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CarsPrice", menuName = "Scriptable Objects/CarsPrice")]
public class CarsPriceConfig : ScriptableObject
{
    public int[] CashPrice;
    public int[] GemPrice;
}
