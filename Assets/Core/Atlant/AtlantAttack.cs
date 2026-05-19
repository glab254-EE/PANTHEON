using UnityEngine;
using System.Collections;

public class AtlantAttack : MonoBehaviour
{
    [SerializeField] private AtlantController atlantController;
    [SerializeField] private float rotationSpeed = 8f;

    [SerializeField] private string Attack = "Attack";
    [SerializeField] private string AttackType = "AttackType";
    [SerializeField] private string DashAttack = "DashAttack";

    private Animator _animator;
    //private static readonly int Attack = Animator.StringToHash("Attack");
    //private static readonly int AttackType = Animator.StringToHash("AttackType");
    //private static readonly int DashAttack = Animator.StringToHash("DashAttack");

    private void Awake()
    {
        _animator = GetComponent <Animator>();
        if (atlantController == null)
            atlantController = GetComponent <AtlantController> ();
    }

    public void PerformMeleeAttack()
    {
        int randomAttack = Random.Range(0, 3);
        _animator.SetInteger(AttackType, randomAttack);
        _animator.SetTrigger(Attack);

        StartCoroutine(RotateToPlayerRoutine());
    }

    public void PerformDashAttack()
    {
        _animator.SetTrigger(DashAttack);
    }

    private IEnumerator RotateToPlayerRoutine()
    {
        if (atlantController.player == null) yield break;

        Vector3 direction = (atlantController.player.position - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero) yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}