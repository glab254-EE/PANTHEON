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

    [Header("Настройки атаки")]
    [SerializeField] private float attackRange = 2.5f;

    private bool _canMove = true;
    private bool _isAttacking;
    private NavMeshAgent _agent;
    private Coroutine _followRoutine;
    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = stoppingDistance;
        _animator = GetComponent<Animator>();

        if (attack == null)
            attack = GetComponent<AtlantAttack>();

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

            if (_isAttacking)
            {
                yield return new WaitForSeconds(pathUpdateInterval);
                continue;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                _isAttacking = true;
                _agent.isStopped = true;
                _agent.ResetPath();
                _animator.SetBool(IsWalking, false);
                attack.PerformAttack();
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

    private void OnDisable()
    {
        if (_followRoutine != null)
        {
            StopCoroutine(_followRoutine);
            _followRoutine = null;
        }
    }
}