using UnityEngine;

public class PlayerStunningBehaviour : MonoBehaviour
{
    [SerializeField]
    private PlayerCombatBehaviour playerCombatBehaviour;
    [SerializeField]
    private PlayerMovementController playerMovementController;
    [SerializeField]
    private AnimatorHandler handler;
    public void OnPlayerStunned()
    {
        playerCombatBehaviour.CanAttack = false;
        playerMovementController.OverrideTargetSpeed = Vector3.zero;
        handler.SetAnimatorTrigger("Flung");
    }
    public void OnPlayerStandUp()
    {
        playerCombatBehaviour.CanAttack = true;
        playerMovementController.OverrideTargetSpeed = null;
    }
}
