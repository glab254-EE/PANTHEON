using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Set-up")]
    [SerializeField] public EnemyHealth EnemyHealth;
    [SerializeField] private EnemyAtack enemyAtack;
    [Header("Settings")]
    [SerializeField] public LayerMask PlayerMask;
    [SerializeField] public float RotationSpeed = 8f;
    [SerializeField] public float HardAtackNum = 3f;
    [SerializeField] private float updateInterval = 0.3f;

    public bool IsAttacking = false;
    public NavMeshAgent Agent;
    public Animator Animator;
    public Transform Player;
    public bool IsPlayerInTrigger = false;
    public float AtackCount = 0f;

    private bool _isActive = false;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        StartCoroutine(MainCoroutine());
    }

    public void Activate(Transform target)
    {
        Player = target;
        _isActive = true;
    }

    public void DeActivate(Transform target)
    {
        Agent.SetDestination(transform.position);
        _isActive = false;
        Animator.SetBool("EnemyWalk", false);
        Animator.SetTrigger("StayAnimForEnemy");
    }

    IEnumerator MainCoroutine()
    {
        while (true)
        {
            if (EnemyHealth.Health <= 0)
            {
                yield break;
            }
            if (_isActive && Player != null && !IsPlayerInTrigger && !IsAttacking && Agent.isActiveAndEnabled)
            {
                Animator.SetBool("EnemyWalk", true);
                Agent.SetDestination(Player.position);
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _isActive)
        {
            IsPlayerInTrigger = true;

            if (!IsAttacking)
            {
                Animator.SetBool("EnemyWalk", false);
                Animator.SetTrigger("StayAnimForEnemy");
                enemyAtack.AttackSequence();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInTrigger = false;
        }
    }
}