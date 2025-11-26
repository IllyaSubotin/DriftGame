using UnityEngine;
using System;
using System.Collections;

public class OfflineTimer : MonoBehaviour, IOfflineTimer
{
    public float TimeLeft { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action<float> OnTimeChanged;
    public event Action OnTimerEnded;

    private Coroutine timerRoutine;

    public void StartTimer(float duration)
    {
        TimeLeft = duration;
        IsRunning = true;

        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        timerRoutine = StartCoroutine(TimerRoutine());
    }

    public void Pause()
    {
        IsRunning = false;
    }

    public void Resume()
    {
        IsRunning = true;

        if (timerRoutine == null)
            timerRoutine = StartCoroutine(TimerRoutine());
    }

    public void Stop()
    {
        IsRunning = false;
        TimeLeft = 0;

        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        OnTimeChanged?.Invoke(TimeLeft);
        OnTimerEnded?.Invoke();
    }

    private IEnumerator TimerRoutine()
    {
        while (IsRunning && TimeLeft > 0f)
        {
            TimeLeft -= Time.deltaTime;
            OnTimeChanged?.Invoke(TimeLeft);

            yield return null;
        }

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0;
            IsRunning = false;

            OnTimeChanged?.Invoke(TimeLeft);
            OnTimerEnded?.Invoke();
        }

        timerRoutine = null;
    }
}
