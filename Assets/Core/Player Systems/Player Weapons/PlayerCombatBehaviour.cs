using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCombatBehaviour : MonoBehaviour
{
    [Header("Set-up")]
    [field:SerializeField]
    private PlayerInputListener InputListener;
    [field:SerializeField]
    private AnimatorHandler PlayerAnimatorHandler;
    [field: SerializeField]
    private PlayerMovementController movementController;
    [field:SerializeField]
    private PlayerHealthHandler HealthHandler;
    [field:SerializeField]
    private Transform ToolObjectParent;
    [field:SerializeField]
    private Transform HitboxReference;
    [field:SerializeField]
    private LayerMask EnemyMask;
    [field:SerializeField]
    private InputActionReference AttackActionReference;
    [Header("Animations")]
    [field:SerializeField]
    private string HurtAnimationTriggerName;
    [field:SerializeField]
    private string DeadAnimationBoolName;
    [field:SerializeField]
    private RuntimeAnimatorController defaultAnimator;
    [Header("Tools")]
    [field:SerializeField]
    private List<PlayerWeaponSO> AvailableTools;
    [field:SerializeField]
    private PlayerWeaponSO CurrentTool = null;
    private GameObject CurrentToolVisual;
    private float Cooldown = 0;
    private float ComboTimer = 0;
    private int currentComboIndex = 0;
    private bool CanAttack = true;
    private bool IsLMBHeld = false;
    void Start()
    {
        HealthHandler.OnDamaged += OnPlayerDamaged;
        InputListener.ConnectEventToKeybind(AttackActionReference, OnLeftMousePressOrUp, true);
    }
    void Update()
    {
        if (HealthHandler.Health <= 0) return;
        HandleToolVisual();
        if (Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
        }
        if (CurrentTool != null && CanAttack)
        {
            if (Cooldown <= 0 && IsLMBHeld)
            {
                List<AttackPattern> patternToPick = CurrentTool.AttackCombos;
                if (InputListener.EnpowerButtonHeld)
                {
                    patternToPick = CurrentTool.HeavyAttackCombos;
                }
                StartCoroutine(HandleAttacks(patternToPick, CurrentTool.ComboDuration));
            }
        }
        if (ComboTimer > 0)
        {
            ComboTimer -= Time.deltaTime;
        }
        if (currentComboIndex != 0 && ComboTimer <= 0)
        {
            currentComboIndex = 0;
        }
    }
    IEnumerator HandleAttacks(List<AttackPattern> patterns, float comboTimerValue)
    {
        if (patterns.Count > 0 && patterns.Count > currentComboIndex)
        {
            ComboTimer = comboTimerValue;
            AttackPattern pattern = patterns[currentComboIndex];
            if (pattern != null)
            {
                movementController.OverrideTargetSpeed = Vector3.zero;
                CanAttack = false;
                foreach (AttackSettings attack in pattern.Pattern)
                {
                    PlayerAnimatorHandler.SetAnimatorIntFrame(attack.AttackAnimationPropertyName,attack.AttackAnimationPropertyIndex);
                    Cooldown = attack.AttackWindupTime + attack.Duration + attack.Cooldown + 1.75f; // 'failsafe' for cooldown.
                    yield return new WaitForSeconds(attack.AttackWindupTime);
                    PlayerAnimatorHandler.SetAnimatorIntFrame(attack.AttackAnimationPropertyName, 0);
                    HandleHit(attack);
                    yield return new WaitForSeconds(attack.Duration);
                    Cooldown = attack.Cooldown;
                }
                currentComboIndex = Mathf.Clamp(currentComboIndex + 1, 0, patterns.Count - 1);
                movementController.OverrideTargetSpeed = null;
                CanAttack = true;
            }
        }
    }
    void HandleHit(AttackSettings attack)
    {
        if (attack.Damage > 0 && attack.HitboxSize.magnitude > 0 && HitboxReference != null)
        {
            Vector3 origin = HitboxReference.position;

            origin += HitboxReference.forward * attack.HitboxOffset.z;
            origin += HitboxReference.right * attack.HitboxOffset.x;
            origin += HitboxReference.up * attack.HitboxOffset.y;

            bool HaveHitSomething = StatcHitboxCreator.TryHitWithBoxHitbox(origin,attack.HitboxSize,EnemyMask,attack.Damage,false,HitboxReference.rotation);
            Debug.Log(HaveHitSomething);
        }
    }
    void HandleToolVisual()
    {
        if (CurrentToolVisual == null && CurrentTool != null && CurrentTool.model != null)
        {
            CurrentToolVisual = Instantiate(CurrentTool.model,ToolObjectParent);
            HandleAnimator();
        } else if (CurrentToolVisual != null && (CurrentTool == null || CurrentTool.model == null))
        {
            Destroy(CurrentTool);
            HandleAnimator();
        }
    }
    void HandleAnimator()
    {
        RuntimeAnimatorController controller = defaultAnimator;
        if (CurrentTool != null && CurrentTool.animator != null)
        {
           controller = CurrentTool.animator;
        }
        PlayerAnimatorHandler.SetAnimator(controller);
    }
    void OnLeftMousePressOrUp(InputAction.CallbackContext callbackContext)
    {
        IsLMBHeld = callbackContext.ReadValueAsButton();
    }
    void OnPlayerDamaged(double newHealth)
    {
        if (newHealth <= 0 && DeadAnimationBoolName != null)
        {
            PlayerAnimatorHandler.SetAnimatorBool(DeadAnimationBoolName,true);
            Task.Run(DeathTask);
        } else
        {
            PlayerAnimatorHandler.SetAnimatorTrigger(HurtAnimationTriggerName);            
        }
    }
    async Task DeathTask()
    {
        await Task.Delay(2000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
