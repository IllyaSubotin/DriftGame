using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCarParameters", menuName = "Scriptable Objects/CarParameter")]
public class CarConfig : ScriptableObject
{
    public int maxSpeed;
    public int maxReverseSpeed;
    public int motorPower;
    public int brakePower;
    public int maxSteerAngle;
    public int decelerationMultiplier;
    public int driftMultiplier;
}
