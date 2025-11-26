using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreController : MonoBehaviour, IScoreController
{
    [HideInInspector] public int currentScore { get; set; }
    public event Action<float> OnScoreChanged;
    private ICarController _carController;

    public void Init(ICarController carController)
    {
        _carController = carController;
    }

    public void RestartScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }

    private void Update()
    {
        if (_carController.isDrifting)
        {
            currentScore += 1;

            if (_carController.driftAmount > 0.5)
                currentScore += 2;

            if (_carController.speed > 15)
                currentScore += 2;

            OnScoreChanged?.Invoke(currentScore);
        }
        
    }


}
