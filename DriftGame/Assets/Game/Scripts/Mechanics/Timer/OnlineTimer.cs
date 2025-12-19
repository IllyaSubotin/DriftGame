using UnityEngine;
using Photon.Pun;
using System;
using System.Collections;

public class Timer : MonoBehaviourPun, ITimerOnline
{
    public float TimeLeft { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action<float> OnTimeChanged;
    public event Action OnTimerEnded;

    private Coroutine timerRoutine;

    public void StartTimer(float duration)
    {
        if (!PhotonNetwork.IsMasterClient)
            return; 

        TimeLeft = duration;
        IsRunning = true;

        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        timerRoutine = StartCoroutine(TimerRoutine());
    }

    public void Pause()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        IsRunning = false;
    }

    public void Resume()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        IsRunning = true;

        if (timerRoutine == null)
            timerRoutine = StartCoroutine(TimerRoutine());
    }

    public void Stop()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        IsRunning = false;
        TimeLeft = 0;

        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        photonView.RPC("RPC_UpdateTime", RpcTarget.All, TimeLeft);
        photonView.RPC("RPC_Finish", RpcTarget.All);
    }

    private IEnumerator TimerRoutine()
    {
        while (IsRunning && TimeLeft > 0f)
        {
            TimeLeft -= Time.deltaTime;

            // Хост розсилає час
            photonView.RPC("RPC_UpdateTime", RpcTarget.All, TimeLeft);

            yield return null;
        }

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0;
            IsRunning = false;

            photonView.RPC("RPC_Finish", RpcTarget.All);
        }
    }

    // Клієнти оновлюють UI
    [PunRPC]
    private void RPC_UpdateTime(float newTime)
    {
        TimeLeft = newTime;
        OnTimeChanged?.Invoke(TimeLeft);
    }

    [PunRPC]
    private void RPC_Finish()
    {
        OnTimerEnded?.Invoke();
    }
}
