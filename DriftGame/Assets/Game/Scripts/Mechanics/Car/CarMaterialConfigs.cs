using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarMaterial", menuName = "Scriptable Objects/CarMaterial")]
public class CarMaterialConfig : ScriptableObject
{
    public Material[] materials;
}
