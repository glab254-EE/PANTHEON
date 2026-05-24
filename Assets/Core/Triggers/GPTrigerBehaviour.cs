using UnityEngine;
using UnityEngine.Events;

public class GPTrigerBehaviour : MonoBehaviour
{
    [SerializeField]
    private int ActivationAmount = 1;
    [SerializeField]
    private LayerMask MaskToDetect;
    [SerializeField]
    private UnityEvent<Transform> EnterEvents;
    [SerializeField]
    private UnityEvent<Transform> ExitEvents;
    void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeInHierarchy) return;
        if (other.transform != null && other.gameObject != null  && LayerMaskUtils.CompareCollisionlayers(other.gameObject.layer,MaskToDetect) && (ActivationAmount == -1 || ActivationAmount > 0))
        {
            EnterEvents.Invoke(other.transform);
            if (ActivationAmount > 0) ActivationAmount -= 1;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (!gameObject.activeInHierarchy) return;
        if (other.transform != null && other.gameObject != null && LayerMaskUtils.CompareCollisionlayers(other.gameObject.layer,MaskToDetect) && (ActivationAmount == -1 || ActivationAmount > 0))
        {
            ExitEvents.Invoke(other.transform);
            if (ActivationAmount > 0) ActivationAmount -= 1;
        }
    }
}
