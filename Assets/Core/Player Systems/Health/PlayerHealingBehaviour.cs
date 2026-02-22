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
    private PlayerInputListener Listener;
    [SerializeField]
    private InputActionReference BlockAction;
    [SerializeField]
    private double HealingPower = 1;
    [SerializeField]
    private float HealingStayDurationSeconds = 1;
    [SerializeField]
    private float HealingCooldownSeconds = 1;
    [SerializeField]
    private string HealAnimationTriggerName = "Block";
    private bool onCooldown = false;
    private void Start()
    {
        Listener.ConnectEventToKeybind(BlockAction, OnBlockToggle, true, false);
    }
    private void OnBlockToggle(InputAction.CallbackContext context)
    {
        if (!onCooldown && context.ReadValueAsButton())
        {
            onCooldown = true;
            Animation_Handler.SetAnimatorTrigger(HealAnimationTriggerName);
            Player_Health.TryDamage(-HealingPower,null);
            Player_MovementController.OverrideTargetSpeed = Vector3.zero;
            StartCoroutine(StayStillEnumerator());
        }
    }
    private IEnumerator StayStillEnumerator()
    {
        yield return new WaitForSeconds(HealingStayDurationSeconds);
        Player_MovementController.OverrideTargetSpeed = null;
        yield return new WaitForSeconds(HealingCooldownSeconds);
        onCooldown = false;
    }
}
