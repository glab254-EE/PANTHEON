using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealingBehaviour : MonoBehaviour
{
    [SerializeField]
    private AnimatorHandler Animation_Handler;
    [SerializeField]
    private PlayerHealthHandler Player_Health;
    [SerializeField]
    private PlayerMovementController Player_MovementController;
    [SerializeField]
    private int HealingOrbIndex;
    [SerializeField]
    private double HealthPerOrbAddition = 2;
    [SerializeField]
    private PlayerInputListener Listener;
    [SerializeField]
    private InputActionReference HealAction;
    [SerializeField]
    private double HealingPower = 1;
    [SerializeField]
    private float HealingStayDurationSeconds = 1;
    [SerializeField]
    private float HealingCooldownSeconds = 1;
    [SerializeField]
    private string HealAnimationTriggerName = "Block";
    private bool onCooldown = false;
    private PlayerStatSO PlayerHealingOrbStatReference;
    private Coroutine coroutine;
    private void Start()
    {
        Listener.ConnectEventToKeybind(HealAction, OnHealAction, true, false);
        if (HealingOrbIndex >= 0 && HealingOrbIndex < PlayerStatisticsManager.Currencies.Count) PlayerHealingOrbStatReference = PlayerStatisticsManager.Currencies[HealingOrbIndex];
    }
    void OnDisable()
    {
       if (coroutine != null)
        {
            StopCoroutine(coroutine);
        } 
    }
    void OnHealAction(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if (!Player_MovementController.IsActing && !onCooldown && context.ReadValueAsButton() && (PlayerHealingOrbStatReference == null || PlayerHealingOrbStatReference.CurrentValue > 0))
        {
            Player_MovementController.IsActing = true;
            onCooldown = true;
            Animation_Handler.SetAnimatorTrigger(HealAnimationTriggerName);
            double ammount = HealingPower;
            if (PlayerHealingOrbStatReference != null)
            {
                double MaxHP = Player_Health.MaxHealth;
                double Needed = Player_Health.MaxHealth - Player_Health.Health;
                double Taken = System.Math.Min(PlayerHealingOrbStatReference.CurrentValue, Needed/HealthPerOrbAddition);
                ammount = Taken*HealthPerOrbAddition;
                PlayerHealingOrbStatReference.CurrentValue -= Taken;
                PlayerHealingOrbStatReference.InvokeEvent();
            }
            Player_Health.TryDamage(-ammount, null,null);
            Player_MovementController.OverrideTargetSpeed = Vector3.zero;
            OnDisable();
            coroutine = StartCoroutine(StayStillEnumerator());
        }
    }
    private IEnumerator StayStillEnumerator()
    {
        yield return new WaitForSeconds(HealingStayDurationSeconds);
        Player_MovementController.OverrideTargetSpeed = null;
        Player_MovementController.IsActing = false;
        yield return new WaitForSeconds(HealingCooldownSeconds);
        onCooldown = false;
    }
}
