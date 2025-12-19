using UnityEngine;

public class GameplayCameraController : MonoBehaviour, ICameraController
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offset From Car")]
    public Vector3 offset = new Vector3(0, 4, -7);

    public void SetTarget(Transform t)
    {
        target = t;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Камера просто слідує за машиною з заданим офсетом
        transform.position = target.TransformPoint(offset);

        // Камера дивиться на машину
        transform.rotation = Quaternion.LookRotation(
            target.position - transform.position,
            Vector3.up
        );
    }
}
