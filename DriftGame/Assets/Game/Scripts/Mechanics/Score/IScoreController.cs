using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreController
{
    int currentScore { get; }

    void Init(ICarController carController);
    void RestartScore();

    event Action<float> OnScoreChanged;
}
