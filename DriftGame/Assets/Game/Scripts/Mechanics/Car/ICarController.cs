using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarController
{
    float speed { get; }
    float driftAmount { get; }
    bool isDrifting { get; }
}