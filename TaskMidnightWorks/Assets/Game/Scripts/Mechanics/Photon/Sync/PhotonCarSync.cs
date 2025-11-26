using Photon.Pun;
using UnityEngine;

public class PhotonCarSync : MonoBehaviourPun, IPunObservable
{
    private Rigidbody rb;

    // Дані, які отримує клієнт
    private Vector3 networkPos;
    private Quaternion networkRot;
    private Vector3 networkVel;

    // Наскільки швидко вирівнюється
    public float positionLerpSpeed = 12f;
    public float rotationLerpSpeed = 10f;
    public float velocityLerpSpeed = 8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine && photonView.Owner != null)
        {
            // Плавно підтягнути позицію
            transform.position = Vector3.Lerp(
                transform.position,
                networkPos,
                Time.fixedDeltaTime * positionLerpSpeed
            );

            // Плавно підтягнути поворот
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                networkRot,
                Time.fixedDeltaTime * rotationLerpSpeed
            );

            // Плавно підтягнути velocity (дає правильні повороти коліс)
            rb.velocity = Vector3.Lerp(
                rb.velocity,
                networkVel,
                Time.fixedDeltaTime * velocityLerpSpeed
            );
        }
    }

    // Обмін даними між клієнтами ↓↓↓
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Відправляємо свої значення іншим гравцям
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.velocity);
        }
        else
        {
            // Отримуємо значення від інших
            networkPos = (Vector3)stream.ReceiveNext();
            networkRot = (Quaternion)stream.ReceiveNext();
            networkVel = (Vector3)stream.ReceiveNext();
        }
    }
}
