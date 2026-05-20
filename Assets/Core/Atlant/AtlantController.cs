using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AtlantController : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] public Transform player;
    [SerializeField] private AtlantAttack attack;

    [Header("Настройки движения")]
    [SerializeField] private float pathUpdateInterval = 0.3f;
    [SerializeField] private float stoppingDistance = 2f;

    [Header("Настройки ближней атаки")]
    [SerializeField] private float attackRange = 2.5f;

    [Header("Настройки рывка-атаки")]
    [SerializeField] private float dashMinRange = 10f;
    [SerializeField] private float dashMaxRange = 15f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashCooldown = 5f;

    [SerializeField] private string IsWalking = "IsWalking";

    private bool _canMove = true;
    private bool _isAttacking;
    private bool _isDashing;
    private float _lastDashTime = -999f;
    private NavMeshAgent _agent;
    private Coroutine _followRoutine;
    private Animator _animator;
    //private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _agent = GetComponent <NavMeshAgent>();
        _agent.stoppingDistance = stoppingDistance;
        _animator = GetComponent <Animator>();

        if (attack == null)
            attack = GetComponent <AtlantAttack>();

        if (_canMove) SetCanMove(true);
    }

    public void SetCanMove(bool value)
    {
        _canMove = value;

        if (_canMove)
        {
            if (_followRoutine == null)
                _followRoutine = StartCoroutine(FollowPlayerRoutine());
        }
        else
        {
            if (_followRoutine != null)
            {
                StopCoroutine(_followRoutine);
                _followRoutine = null;
            }

            if (_agent.isActiveAndEnabled)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }

            _animator.SetBool(IsWalking, false);
        }
    }

    public void EndAttack()
    {
        _isAttacking = false;
    }

    IEnumerator FollowPlayerRoutine()
    {
        yield return null;

        while (true)
        {
            if (player == null || !_agent.isActiveAndEnabled)
            {
                yield return new WaitForSeconds(pathUpdateInterval);
                continue;
            }

            if (_isAttacking || _isDashing)
            {
                yield return new WaitForSeconds(pathUpdateInterval);
                continue;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            bool inDashZone = distanceToPlayer >= dashMinRange && distanceToPlayer <= dashMaxRange;

            if (inDashZone)
            {
                if (Time.time >= _lastDashTime + dashCooldown)
                {
                    int roll = Random.Range(0, 3);

                    if (roll == 1)
                    {
                        yield return StartCoroutine(DashAttackRoutine());
                        continue;
                    }
                }
            }

            if (distanceToPlayer <= attackRange)
            {
                _isAttacking = true;
                _agent.isStopped = true;
                _agent.ResetPath();
                _animator.SetBool(IsWalking, false);
                attack.PerformMeleeAttack();
            }
            else
            {
                _agent.isStopped = false;
                _agent.SetDestination(player.position);

                bool isWalking = _agent.hasPath && _agent.remainingDistance > _agent.stoppingDistance;
                _animator.SetBool(IsWalking, isWalking);
            }

            yield return new WaitForSeconds(pathUpdateInterval);
        }
    }

    IEnumerator DashAttackRoutine()
    {
        _isDashing = true;
        _lastDashTime = Time.time;

        _agent.isStopped = true;
        _agent.enabled = false;

        Vector3 targetPos = player.position;
        Vector3 startPos = transform.position;

        Vector3 direction = (targetPos - startPos).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        attack.PerformDashAttack();

        float distance = Vector3.Distance(startPos, targetPos);
        float dashDuration = distance / dashSpeed;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            transform.position += direction * dashSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        _agent.enabled = true;
        _agent.Warp(transform.position);
        _agent.isStopped = false;

        _isDashing = false;
    }

    private void OnDisable()
    {
        if (_followRoutine != null)
        {
            StopCoroutine(_followRoutine);
            _followRoutine = null;
        }
    }
}