using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Set-Up")]
    [SerializeField]
    private AnimatorHandler animator;
    [SerializeField]
    private PlayerInputListener listener;
    [SerializeField]
    private CameraBehaviour cameraBehaviour;
    [SerializeField]
    private PlayerHealthHandler playerHealth;
    [Header("Input")]
    [SerializeField]
    private InputActionReference ToggleLookForwardBind;
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
    private Vector3 CurrentSpeed = new();
    private Rigidbody rb;
    private float PlayerCurrentMaxSpeed;
    void Start()
    {
        PlayerCurrentMaxSpeed = PlayerMaxSpeed;
        listener.MouseLocked = DefaultEnabledState;
        playerHealth.OnDamaged += OnPlayerDamaged;
        rb = GetComponent<Rigidbody>();
        listener.ConnectEventToKeybind(ToggleLookForwardBind,ToggleLookForward);
        listener.ConnectEventToKeybind(RollKey,OnRollButtonPress);
        listener.ConnectEventToKeybind(RollKey, OnRunPresses,true);
    }
    void Update()
    {
        if (CanMove && IsOnGround(out RaycastHit hit))
        {
            HandleMovement(hit);
            HandleLook();
        }
        HandleAnimations();
        cameraBehaviour.CameraLocked = listener.MouseLocked;
    }
    void OnPlayerDamaged(double currentHealth)
    {
        IsAlive = currentHealth>0;
        CanMove = CanMove && IsAlive;
        CanRoll = IsAlive;
    }
    void HandleMovement(RaycastHit groundHit)
    {
        if (Vector3.Angle(groundHit.normal, Vector3.up) > MaxSlopeAngle && OverrideTargetSpeed == null) return;
        Vector3 targetSpeed = listener.MovementVector3 * PlayerCurrentMaxSpeed;

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
            NoYVector = listener.CameraTransform.forward;
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
            animator.SetAnimatorBool(WalkingBoolName, true);
        }
        else
        {
            animator.SetAnimatorBool(WalkingBoolName, false);
        }
    }
    void ToggleLookForward(InputAction.CallbackContext _)
    {
        LookForward = !LookForward;
    }
    void OnRollButtonPress(InputAction.CallbackContext callbackContext)
    {
        if (CanMove && CanRoll && callbackContext.ReadValueAsButton() && listener.MovementVector3.magnitude > 0)
        {
            animator.SetAnimatorTrigger(RollTriggerName);
            Task.Run(RollTask);
        }
    }
    void OnRunPresses(InputAction.CallbackContext callbackContext)
    {
        if (listener.MovementVector3.magnitude > 0 && CanMove && callbackContext.ReadValueAsButton())
        {
            animator.SetAnimatorBool(RunAnimationBoolName, true);
            PlayerCurrentMaxSpeed = PlayerRunningMaxSpeed;
        } else
        {
            animator.SetAnimatorBool(RunAnimationBoolName, false);
            PlayerCurrentMaxSpeed = PlayerMaxSpeed;
        }
    }
    Task RollTask()
    {

        bool _oldlook = LookForward;


        Vector3 _targetSpeed = listener.MovementVector3.normalized * PlayerRollSpeed;

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
