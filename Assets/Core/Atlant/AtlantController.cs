using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class AtlantController : MonoBehaviour
{
    [Header("Set-up")]
    [SerializeField] public Transform player;
    [SerializeField] private AtlantAttack attack;
    [SerializeField] private RevanController revanController;
    [field:SerializeField] public EnemyHealth health{get;private set;}

    [Header("AI")]
    [SerializeField] private float pathUpdateInterval = 0.3f;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private double healthThresholdForSecondPhase = 1000;
    [SerializeField] private double healthThresholdForThirdPhase = 250;

    [Header("AttackSetting")]
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackCooldown = 0.5f;

    [Header("AttackSetting - dash")]
    [SerializeField] private float dashMinRange = 10f;
    [SerializeField] private float dashMaxRange = 15f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashCooldown = 5f;
    [Header("Animations")]
    [SerializeField] private string IsWalking = "IsWalking";

    private bool _canMove = true;
    //private bool _isAttacking;
    private bool _isDashing;
    private float _lastDashTime = 0;
    private float _lastAttackTime;
    private int CurrentState = 1;
    private NavMeshAgent _agent;
    private Coroutine _followRoutine;
    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent <NavMeshAgent>();
        _agent.stoppingDistance = stoppingDistance;
        _animator = GetComponent <Animator>();

        if (attack == null)
            attack = GetComponent <AtlantAttack>();

        if (_canMove) SetCanMove(true);

        health.OnDamaged += OnHealthChanged;
    }

    private void OnHealthChanged(double newHP)
    {
        if (CurrentState == 1 && newHP <= healthThresholdForSecondPhase)
        {
            CurrentState = 2;
            revanController.Activate();
        } else if (CurrentState == 2 && newHP <= healthThresholdForThirdPhase)
        {
            CurrentState = 3;
            revanController.InSecondPhase = true;
        } else if (newHP <= 0 && CurrentState != -1)
        {
            CurrentState = -1;
            revanController.Die();
        }
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

    //public void EndAttack() { _isAttacking = false; }

    //private static readonly int AttackTag = Animator.StringToHash("Attack");

    IEnumerator FollowPlayerRoutine()
    {
        yield return null;

        while (health.Health > 0)
        {
            if (player == null || !_agent.isActiveAndEnabled)
            {
                yield return new WaitForSeconds(pathUpdateInterval);
                continue;
            }

            bool isAttacking = _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

            if (isAttacking || _isDashing)
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
                    int roll = UnityEngine.Random.Range(0, 3);

                    if (roll == 1)
                    {
                        yield return StartCoroutine(DashAttackRoutine());
                        continue;
                    }
                }
            }

            if (distanceToPlayer <= attackRange && Time.time >= _lastAttackTime + attackCooldown)
            {
                _lastAttackTime = Time.time;
                //_isAttacking = true;
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