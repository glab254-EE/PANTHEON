using UnityEngine;

public class PlayerDeathScreenHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerHealthHandler healthHandler;
    [SerializeField]
    private GameObject UIPrefab;
    [SerializeField]
    private Transform UIParent;
    private GameObject currentscreen;
    void Start()
    {
        healthHandler.OnDamaged += OnHealthChanged;
    }
    void OnHealthChanged(double current)
    {
        if (current <= 0 && currentscreen == null)
        {
            currentscreen = Instantiate(UIPrefab,UIParent);
        }
    }
}
