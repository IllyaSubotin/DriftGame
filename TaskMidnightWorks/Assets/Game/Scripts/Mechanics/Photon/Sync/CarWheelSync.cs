using UnityEngine;
using Photon.Pun;

public class CarWheelSync : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Transform[] wheelMeshes;

    private Vector3[] wheelRotations;  // передаємо тільки локальні кути mesh колеса

    private void Awake()
    {
        wheelRotations = new Vector3[wheelMeshes.Length];
    }

    public void Init(CarWheelSet WheelMeshes)
    {
        wheelMeshes = new Transform[4];
        wheelMeshes[0] = WheelMeshes.fLMesh.transform;
        wheelMeshes[1] = WheelMeshes.fRMesh.transform;
        wheelMeshes[2] = WheelMeshes.rLMesh.transform;
        wheelMeshes[3] = WheelMeshes.rRMesh.transform;
    }

    private void Update()
    {
        // якщо це НЕ наша машина → застосовуємо отримані кути
        if (!photonView.IsMine && photonView.Owner != null)
        {
            for (int i = 0; i < wheelMeshes.Length; i++)
            {
                wheelMeshes[i].localEulerAngles = wheelRotations[i];
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // передаємо кути коліс
            for (int i = 0; i < wheelMeshes.Length; i++)
                stream.SendNext(wheelMeshes[i].localEulerAngles);
        }
        else
        {
            // отримуємо кути
            for (int i = 0; i < wheelMeshes.Length; i++)
                wheelRotations[i] = (Vector3)stream.ReceiveNext();
        }
    }
}
