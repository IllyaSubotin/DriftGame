using Photon.Pun;
using UnityEngine;

public class CarCustomizationSync : MonoBehaviourPun, IPunObservable
{
    public CarCustomizer _customizer;

    private int _carId;
    private int _materialIndex;
    private int _wheelsIndex;
    private bool _spoilerEnabled;

    public void ApplyLocalCustomization(ICarData data)
    {
        if (!photonView.IsMine) return;

        CarSaveData save = data.carSaveDatas[data.currentCarId];

        _carId          = data.currentCarId;
        _materialIndex  = save.materialIndex;
        _wheelsIndex    = save.wheelsIndex;
        _spoilerEnabled = save.spoilerEnabled;

        _customizer.ApplyCustomization(data);
    }

    private void ApplyRemote()
    {
        _customizer.ApplyCustomization(
            new RuntimeCarData(_carId, _materialIndex, _wheelsIndex, _spoilerEnabled)
        );
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_carId);
            stream.SendNext(_materialIndex);
            stream.SendNext(_wheelsIndex);
            stream.SendNext(_spoilerEnabled);
        }
        else
        {
            _carId          = (int)stream.ReceiveNext();
            _materialIndex  = (int)stream.ReceiveNext();
            _wheelsIndex    = (int)stream.ReceiveNext();
            _spoilerEnabled = (bool)stream.ReceiveNext();

            ApplyRemote();
        }
    }
}
