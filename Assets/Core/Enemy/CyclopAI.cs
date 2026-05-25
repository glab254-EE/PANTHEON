using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CyclopAI : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float coolDown = 1f;
    [SerializeField] private float updateInterval = 0.3f;
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private string attack = "Attack";
    [SerializeField] private string attackType = "AttackType";
    [SerializeField] private string isWalking = "IsWalking";
    [SerializeField] private AttackSettings setting;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private EnemyHealth health;
    private Animator _animator;
    private NavMeshAgent _agent;
    private bool _canMove = false;
    private bool _inTrigger = false;
    private bool _inAtack = false;

    private void Start()
    {
        if (_animator == null) { _animator = GetComponent<Animator>(); }
        if (_agent == null) { _agent = GetComponent<NavMeshAgent>(); }
        StartCoroutine(ControllerCoroutine());
    }

    public void Activate()
    {
        _canMove = true;
    }

    public void DeActivate()
    {
        _canMove = false;
    }

    private IEnumerator ControllerCoroutine()
    {
        while (health.Health > 0)
        {
            if (_canMove && !_inTrigger && !_inAtack && _agent != null && _target != null)
            {
                _agent.SetDestination(_target.position);
                _animator.SetBool(isWalking, true);
            }
            else
            {
                _agent.SetDestination(transform.position);
                _animator.SetBool(isWalking, false);
            }

            if (_inTrigger && !_inAtack)
            {
                _canMove = false;
                _inTrigger = true;
                PerformMeleeAttack();                
            }

            yield return new WaitForSeconds(updateInterval);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_inAtack)
        {
            _canMove = false;
            _inTrigger = true;
            PerformMeleeAttack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canMove = true;
            _inTrigger = false;
        }
    }

    public void PerformMeleeAttack()
    {
        if (health.Health <= 0) return;
        if (setting.clip != null && gameObject.TryGetComponent(out AudioSource source))
        {
            source.PlayOneShot(setting.clip);
        }
        _agent.isStopped = true;
        _agent.updateRotation = false;
        int randomAttack = Random.Range(0, 3);
        _animator.SetInteger(attackType, randomAttack);
        _animator.SetTrigger(attack);
        _inAtack = true;
    }

    private IEnumerator RotationToPlayer()
    {
        if (health.Health <= 0) yield break;
        if (_target != null)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                while (Quaternion.Angle(transform.rotation, targetRotation) > 5f)
                {
                    if (health.Health <= 0) yield break;
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    yield return null;
                }
                transform.rotation = targetRotation;
            }
        }
    }

    private IEnumerator EndAtack()
    {
        if (health.Health <= 0) yield break;
        _inAtack = false;
        _agent.isStopped = false;
        _agent.updateRotation = true;
        if (_inTrigger)
        {
            yield return new WaitForSeconds(coolDown);
            PerformMeleeAttack();
        }
    }

    private IEnumerator Atack()
    {   
        if (health.Health <= 0) yield break;
        yield return new WaitForSeconds(setting.AttackWindupTime);

        Vector3 hitboxOrigin = transform.position;

        hitboxOrigin += transform.forward * setting.HitboxOffset.z;
        hitboxOrigin += transform.right * setting.HitboxOffset.x;
        hitboxOrigin += transform.up * setting.HitboxOffset.y;

        bool haveHitPlayer = StatcHitboxCreator.TryHitWithBoxHitbox(hitboxOrigin, setting.HitboxSize, playerMask, setting.Damage, gameObject, true, transform.rotation, setting.effect);
    }
}