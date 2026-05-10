using System.Collections;
using UnityEngine;

public class AtlantAttack : MonoBehaviour
{
    [SerializeField] private AtlantController atlantController;
    [SerializeField] private float RotationSpeed = 1.0f;

    private Animator _animator;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int AttackType = Animator.StringToHash("AttackType");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PerformAttack()
    {
        int randomAttack = Random.Range(0, 2);

        _animator.SetInteger(AttackType, randomAttack);
        _animator.SetTrigger(Attack);
    }

    private IEnumerator RotationAtlant()
    {
        if (atlantController.player != null)
        {
            Vector3 direction = (atlantController.player.position - transform.position).normalized;
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
    }
}