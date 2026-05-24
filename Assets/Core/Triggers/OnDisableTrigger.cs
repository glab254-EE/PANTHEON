using UnityEngine;
using UnityEngine.Events;

public class OnDisableTrigger : MonoBehaviour
{
    [SerializeField]
    private int ActivationAmount = 1;
    [SerializeField]
    private UnityEvent Events;
    void OnDisable()
    {
        if (ActivationAmount == -1 || ActivationAmount > 0)
        {
            Events?.Invoke();
            ActivationAmount -= 1;
        }
    }
}
