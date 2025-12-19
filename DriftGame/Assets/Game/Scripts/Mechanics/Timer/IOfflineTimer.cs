using System;

public interface IOfflineTimer
{
    float TimeLeft { get; }
    bool IsRunning { get; }

    event Action<float> OnTimeChanged;
    event Action OnTimerEnded;

    void StartTimer(float duration);
    void Pause();
    void Resume();
    void Stop();
}
