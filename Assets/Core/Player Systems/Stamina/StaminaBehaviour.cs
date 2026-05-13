using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBehaviour : MonoBehaviour
{
    [SerializeField]
    private Image StaminaBarImage; //Edited
    [SerializeField]
    private float MaxStamina = 100;
    [SerializeField]
    private int StaminaGainingStartDelay = 100;
    [SerializeField]
    private int TargetAttributeIndex = -1;
    [SerializeField]
    public float StaminaGainPerDelay = 1;
    [SerializeField]
    private float StaminaDelay = 0.1f;
    internal float Stamina { get; private set; }
    internal bool CanReplenishStamina = true;
    internal event System.Action<double> OnStaminaChange;
    private bool isGaining = false;
    void Awake()
    {
        Stamina = MaxStamina;

        if (StaminaBarImage != null)
        {
            StaminaBarImage.fillAmount = (float)(Stamina / MaxStamina); //Edited
        }
        if (TargetAttributeIndex != -1 && PlayerStatisticsManager.TryGetValue(TargetAttributeIndex, out double value, out PlayerStatSO stat))
        {
            MaxStamina = (float)value;
            stat.OnUpdate += OnUpdate;
        }
    }
    public bool TryTakeStamina(float ammount)
    {
        if (Stamina - ammount < 0) return false;

        Stamina = Mathf.Clamp(Stamina - ammount, 0, MaxStamina);
        if (StaminaBarImage != null)
        {
            StaminaBarImage.fillAmount = (float)(Stamina / MaxStamina); //Edited
        }
        isGaining = false;
        if (CanReplenishStamina)
        {
            StartCoroutine(GainEnumerator());
        }
        return true;
    }
    private void OnUpdate(double newval)
    {
        MaxStamina = (float)newval;
    }
    private IEnumerator GainEnumerator()
    {
        Debug.Log("Start");
        yield return new WaitForSeconds(StaminaGainingStartDelay);
        isGaining = true;
        while(isGaining && Stamina < MaxStamina)
        {
            Stamina = Mathf.Clamp(Stamina + StaminaGainPerDelay, 0, MaxStamina);
            if (StaminaBarImage != null)
            {
                StaminaBarImage.fillAmount = (float)(Stamina / MaxStamina); //Edited
            }
            yield return new WaitForSeconds(StaminaDelay);

        }
    }
}
