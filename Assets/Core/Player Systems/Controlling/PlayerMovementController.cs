using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Set-Up")]
    [SerializeField]
    private AnimatorHandler Animator;
    [SerializeField]
    private PlayerInputListener Listener;
    [SerializeField]
    private CameraBehaviour CameraBehaviour;
    [SerializeField]
    private PlayerHealthHandler PlayerHealth;
    [SerializeField]
    private StaminaBehaviour PlayerStaminaBehaviour;
    [Header("Input")]
    [SerializeField]
    private InputActionReference RollKey;
    [SerializeField]
    private bool DefaultEnabledState = true;
    [Header("Animations")]
    [SerializeField]
    private string RunAnimationBoolName = "Walking";
    [SerializeField]
    private string WalkingBoolName = "Running";
    [SerializeField]
    private string RollTriggerName = "Roll";
    [Header("Stats")]
    [SerializeField]
    private float PlayerMaxSpeed;
    [SerializeField]
    private float PlayerRunningMaxSpeed;
    [SerializeField]
    private float PlayerAcceloration = 2;
    [SerializeField]
    private float PlayerTurnSpeed = 10;
    [SerializeField]
    private float PlayerRollSpeed = 4;
    [SerializeField]
    private float PlayerRollDuration = 1f;
    [SerializeField]
    private float PlayerRollForceDuration = 1f;
    [SerializeField]
    private float PlayerRollCooldown = 1f;
    [SerializeField]
    private float PlayerRollStaminaCost = 15f;
    [SerializeField]
    private float PlayerRunStaminaCostPerUpdate = 5f;
    [Header("Other Settings")]
    [SerializeField]
    private float MoveAnimationThreshold = 0.1f;
    [SerializeField]
    private float AdditionalGroundCheckingRayDistance = 0.1f;
    [SerializeField]
    private float MaxSlopeAngle = 15f;
    internal bool LookForward = false;
    internal bool CanMove {get;private set;} = true;
    internal bool CanRoll {get;private set;} = true;
    internal Vector3? OverrideTargetSpeed = null;
    private bool IsAlive = true;
    private bool IsRunning = false;
    private Vector3 CurrentSpeed = new();
    private Rigidbody rb;
    private float PlayerCurrentMaxSpeed;
    void Start()
    {
        PlayerCurrentMaxSpeed = PlayerMaxSpeed;
        Listener.MouseLocked = DefaultEnabledState;
        PlayerHealth.OnDamaged += OnPlayerDamaged;
        rb = GetComponent<Rigidbody>();
        //Listener.ConnectEventToKeybind(ToggleLookForwardBind,ToggleLookForward);
        Listener.ConnectEventToKeybind(RollKey,OnRollButtonPress);
        Listener.ConnectEventToKeybind(RollKey, OnRunPresses,true);
    }
    void Update()
    {
        if (IsAlive && CanMove && IsOnGround(out RaycastHit hit))
        {
            HandleMovement(hit);
            HandleLook();
            HandlePlayerRunningChecks();
        }
        HandleAnimations();
        CameraBehaviour.CameraLocked = Listener.MouseLocked;
    }
    void OnPlayerDamaged(double currentHealth)
    {
        IsAlive = currentHealth>0;
        CanMove = CanMove && IsAlive;
        CanRoll = IsAlive;
    }
    void HandlePlayerRunningChecks()
    {
        if (IsRunning)
        {
            if (rb.linearVelocity.magnitude < MoveAnimationThreshold)
            {
                ToggleRunning(false);
            }
            else
            {
                if (!PlayerStaminaBehaviour.TryTakeStamina(PlayerRunStaminaCostPerUpdate * Time.deltaTime))
                {
                    ToggleRunning(false);
                }
            }
        }
    }
    void HandleMovement(RaycastHit groundHit)
    {
        if (Vector3.Angle(groundHit.normal, Vector3.up) > MaxSlopeAngle && OverrideTargetSpeed == null) return;
        Vector3 targetSpeed = Listener.MovementVector3 * PlayerCurrentMaxSpeed;

        if (OverrideTargetSpeed != null) targetSpeed = (Vector3)OverrideTargetSpeed;

        targetSpeed.y = CurrentSpeed.y;

        Vector3 slerpedSpeed = Vector3.Slerp(CurrentSpeed,targetSpeed,PlayerAcceloration*Time.deltaTime);
        CurrentSpeed = slerpedSpeed;

        Vector3 planarVector3 = Vector3.ProjectOnPlane(slerpedSpeed, groundHit.normal);

        if (OverrideTargetSpeed != null)
        {
            rb.linearVelocity = slerpedSpeed;
        } else
        {
            rb.linearVelocity = planarVector3;
        }

    }
    bool IsOnGround(out RaycastHit hit)
    {
        if (Physics.Raycast(new(transform.position+Vector3.up * .1f,Vector3.down),out hit,transform.localScale.y/2+AdditionalGroundCheckingRayDistance))
        {
            return true;
        }
        return false;
    }
    void HandleLook()
    {
        if (OverrideTargetSpeed != null && LookForward == false) return;
        Vector3 NoYVector = rb.linearVelocity;

        if (LookForward)
        {
            NoYVector = Listener.CameraTransform.forward;
        }

        NoYVector.y = 0;

        NoYVector.Normalize();

        if (NoYVector == Vector3.zero)
            return;

        Quaternion targetLookVector = Quaternion.LookRotation(NoYVector);

        transform.rotation = Quaternion.Slerp(transform.rotation,targetLookVector,PlayerTurnSpeed*Time.deltaTime);
    }
    void HandleAnimations()
    {
        if (CurrentSpeed.magnitude > MoveAnimationThreshold)
        {
            Animator.SetAnimatorBool(WalkingBoolName, true);
        }
        else
        {
            Animator.SetAnimatorBool(WalkingBoolName, false);
        }
    }
    void ToggleLookForward(InputAction.CallbackContext _)
    {
        LookForward = !LookForward;
    }
    void OnRollButtonPress(InputAction.CallbackContext callbackContext)
    {
        if (CanMove && CanRoll && callbackContext.ReadValueAsButton() && Listener.MovementVector3.magnitude > 0 && PlayerStaminaBehaviour.TryTakeStamina(PlayerRollStaminaCost))
        {
            Animator.SetAnimatorTrigger(RollTriggerName);
            Task.Run(RollTask);
        }
    }
    void ToggleRunning(bool isRunning)
    {
        PlayerStaminaBehaviour.CanReplenishStamina = !isRunning;
        PlayerStaminaBehaviour.TryTakeStamina(0);
        IsRunning = isRunning;
        Animator.SetAnimatorBool(RunAnimationBoolName, isRunning);
        if (Listener.MovementVector3.magnitude > 0 && CanMove && isRunning)
        {
            PlayerCurrentMaxSpeed = PlayerRunningMaxSpeed;
        }
        else
        {
            PlayerCurrentMaxSpeed = PlayerMaxSpeed;
        }
    }
    void OnRunPresses(InputAction.CallbackContext callbackContext)
    {
        ToggleRunning(callbackContext.ReadValueAsButton());
    }
    Task RollTask()
    {

        bool _oldlook = LookForward;


        Vector3 _targetSpeed = Listener.MovementVector3.normalized * PlayerRollSpeed;

        CanRoll = false;

        LookForward = false;

        OverrideTargetSpeed = _targetSpeed;

        Task.Delay(Mathf.RoundToInt(PlayerRollForceDuration * 1000)).Wait();

        OverrideTargetSpeed = null;

        Task.Delay(Mathf.RoundToInt(PlayerRollDuration * 1000)).Wait();

        LookForward = _oldlook;

        Task.Delay(Mathf.RoundToInt(PlayerRollCooldown * 1000)).Wait();

        CanRoll = true;

        return Task.CompletedTask;
    }
}
