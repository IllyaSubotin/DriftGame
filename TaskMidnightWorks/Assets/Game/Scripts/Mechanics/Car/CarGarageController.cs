using UnityEngine;
using System.Collections;
using TMPro;

public class CarGarageController : MonoBehaviour
{
    [Header("MAIN")]
    [SerializeField] private Renderer[] _carRenderer;

    [Header("WHEELS")]
    [SerializeField] private GameObject[] _wheelsSets;

    [Header("SPOILER")]
    [SerializeField] private GameObject _spoiler;
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody _rigidbody;


    public bool IsSpoiler() => _spoiler != null;

    public void SetMaterial(Material material)
    {
        foreach (Renderer carRenderer in _carRenderer)
            carRenderer.material = material;
        
        AnimChange();        
    }

    public void SetWheels(int index)
    {
        for (int i = 0; i < _wheelsSets.Length; i++)
            _wheelsSets[i].SetActive(i == index);

        AnimChange();
    }

    public bool EnableSpoiler()
    {
        _spoiler.SetActive(!_spoiler.activeSelf);
        AnimChange();

        return !_spoiler.activeSelf;
    }

    

    public void SetPosition(Vector3 destination)
    {
        transform.position = destination;
    }

    public void MoveToPoint(Vector3 destination, float duration = 3f)
    {
        StartCoroutine(MovePoint(destination, duration));
    }


    private IEnumerator MovePoint(Vector3 destination, float duration)
    {
        Vector3 startPos = transform.position;
        _rigidbody.isKinematic = true;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float k = t / duration;

            k = Mathf.SmoothStep(0, 1, k);

            transform.position = Vector3.Lerp(startPos, destination, k);
            yield return null;
        }
    }
    
    private void AnimChange()
    {
        _rigidbody.isKinematic = false;
        gameObject.transform.position += new Vector3(0, 2, 0);
    }

    
}
