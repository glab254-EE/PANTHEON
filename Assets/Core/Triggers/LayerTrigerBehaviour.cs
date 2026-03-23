using UnityEngine;
using UnityEngine.Events;

public class LayerTrigerBehaviour : MonoBehaviour
{
    [SerializeField]
    private LayerMask MaskToDetect;
    [SerializeField]
    private UnityEvent<Transform> EnterEvents;
    [SerializeField]
    private UnityEvent<Transform> ExitEvents;
    void OnTriggerEnter(Collider other)
    {
        if (other.transform != null && other.gameObject != null && other.gameObject.layer == MaskToDetect)
        {
            EnterEvents.Invoke(other.transform);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform != null && other.gameObject != null && other.gameObject.layer == MaskToDetect)
        {
            ExitEvents.Invoke(other.transform);
        }
    }
}
