using UnityEngine;
using System.Collections;

public class AtlantAttack : MonoBehaviour
{
    [SerializeField] private AtlantController atlantController;
    [SerializeField] private float rotationSpeed = 8f;

    [SerializeField] private string Attack = "Attack";
    [SerializeField] private string AttackType = "AttackType";
    [SerializeField] private string DashAttack = "DashAttack";
    [SerializeField] private AttackSettings Attack1Setting;
    [SerializeField] private AttackSettings Attack2Setting;
    [SerializeField] private AttackSettings Attack3Setting;
    [SerializeField] private LayerMask playerMask;

    private Animator _animator;

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
    }

    public void OnAnimationHit(int typo)
    {
        AttackSettings setting = Attack1Setting;
        switch (typo)
        {
            case 1:
                setting = Attack2Setting;
                break;
            case 2:
                setting = Attack3Setting;
                break;
        }
        Vector3 origin = transform.position;
        origin += transform.right * setting.HitboxOffset.x;
        origin += transform.up * setting.HitboxOffset.y;
        origin += transform.forward * setting.HitboxOffset.z;
        StatcHitboxCreator.TryHitWithBoxHitbox(origin,setting.HitboxSize,playerMask,setting.Damage,gameObject,false,transform.rotation,setting.effect);
    }

    public void PerformDashAttack()
    {
        _animator.SetTrigger(DashAttack);
    }

    private IEnumerator RotateToPlayerRoutine()
    {
        if (atlantController.player == null || atlantController.health.Health <=0) yield break;

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