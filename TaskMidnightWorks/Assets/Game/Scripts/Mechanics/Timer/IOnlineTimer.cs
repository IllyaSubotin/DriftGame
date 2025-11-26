using System;

public interface ITimerOnline
{
    float TimeLeft { get; }
    bool IsRunning { get; }

    void StartTimer(float duration);
    void Pause();
    void Resume();
    void Stop();

    event Action<float> OnTimeChanged;
    event Action OnTimerEnded;
}
