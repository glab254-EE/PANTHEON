using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlockingBehaviour : MonoBehaviour
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
    private double BlockingDamageMultiSetter = .75;
    [SerializeField]
    private string BlockBoolName = "Block";
    private bool blocking = false;
    private double BaseDamageMulti;
    private void Start()
    {
        BaseDamageMulti = Player_Health.DamageMultiplier;

        Listener.ConnectEventToKeybind(BlockAction, OnBlockToggle, true, false);
    }
    private void OnBlockToggle(InputAction.CallbackContext context)
    {
        blocking = context.ReadValueAsButton();

        if (blocking)
        {
            Animation_Handler.SetAnimatorBool(BlockBoolName, true);
            Player_Health.DamageMultiplier = BlockingDamageMultiSetter;
            Player_MovementController.OverrideTargetSpeed = Vector3.zero;
        } else
        {
            Animation_Handler.SetAnimatorBool(BlockBoolName, false);
            Player_Health.DamageMultiplier = BaseDamageMulti;
            Player_MovementController.OverrideTargetSpeed = null;
        }
    }
}
