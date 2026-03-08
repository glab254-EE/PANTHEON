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
    [field: SerializeField] public AttackSettings AttackSetting;
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
    IEnumerator MainCoroutine()
    {
        while (true)
        {
            if (EnemyHealth.Health <= 0)
            {
                Destroy(gameObject);
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
                StartCoroutine(enemyAtack.AttackSequence());
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

    /*IEnumerator AttackSequence()
    {
        IsAttacking = true;
        Agent.isStopped = true;

        bool originalUpdateRotation = Agent.updateRotation;

        if (EnemyHealth.Health <= 0) yield break;

        Agent.updateRotation = false;

        if (_atackCount <= hardAtackNum)
        {
            Animator.SetTrigger("EnemyAtack");
            yield return new WaitForSeconds(AttackSetting.AttackWindupTime);
        }
        else
        {
            Animator.SetTrigger("HardEnemyAtack");
            yield return new WaitForSeconds(AttackSetting.AttackWindupTime);
        }

        Animator.SetTrigger("EnemyAtack");
        yield return new WaitForSeconds(AttackSetting.AttackWindupTime);

        Vector3 hitboxOrigin = transform.position;

        hitboxOrigin += transform.forward * AttackSetting.HitboxOffset.z;
        hitboxOrigin += transform.right * AttackSetting.HitboxOffset.x;
        hitboxOrigin += transform.up * AttackSetting.HitboxOffset.y;

        bool haveHitPlayer = StatcHitboxCreator.TryHitWithBoxHitbox(hitboxOrigin, AttackSetting.HitboxSize, PlayerMask, AttackSetting.Damage, true, transform.rotation);

        if (haveHitPlayer)
        {
            Debug.Log("Hit");
        }

        yield return new WaitForSeconds(AttackSetting.Duration);
        Animator.SetTrigger("StayAnimForEnemy");

        if (Player != null)
        {
            Vector3 direction = (Player.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                while (Quaternion.Angle(transform.rotation, targetRotation) > 5f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
                    yield return null;
                }
                transform.rotation = targetRotation;
            }
        }

        if (IsPlayerInTrigger && Player != null)
        {
            yield return new WaitForSeconds(AttackSetting.Cooldown);
            IsAttacking = false;
            StartCoroutine(AttackSequence());
        }
        else
        {
            IsAttacking = false;
            Agent.updateRotation = true;
            Agent.isStopped = false;
        }
    }*/
}