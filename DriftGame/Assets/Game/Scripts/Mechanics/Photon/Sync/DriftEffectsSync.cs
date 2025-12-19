using Photon.Pun;
using UnityEngine;

public class DriftEffectsSync : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private ParticleSystem smokeLeft;
    [SerializeField] private ParticleSystem smokeRight;

    [SerializeField] private TrailRenderer skidLeft;
    [SerializeField] private TrailRenderer skidRight;

    private bool isDrifting;
    private float driftAmount;

    public void SetDriftState(bool drifting, float amount)
    {
        if (!photonView.IsMine) return;

        isDrifting = drifting;
        driftAmount = amount;
    }

    private void Update()
    {
        if (photonView.IsMine) return;

        if (isDrifting)
        {
            if (!smokeLeft.isPlaying) smokeLeft.Play();
            if (!smokeRight.isPlaying) smokeRight.Play();

            skidLeft.emitting = true;
            skidRight.emitting = true;
        }
        else
        {
            smokeLeft.Stop();
            smokeRight.Stop();

            skidLeft.emitting = false;
            skidRight.emitting = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isDrifting);
            stream.SendNext(driftAmount);
        }
        else
        {
            isDrifting = (bool)stream.ReceiveNext();
            driftAmount = (float)stream.ReceiveNext();
        }
    }
}
