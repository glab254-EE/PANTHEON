using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAtack : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [field: SerializeField] private AttackSettings NormalAttackSetting;
    [field: SerializeField] private AttackSettings HardAttackSetting;

    private void Start()
    {
        enemyAI.AtackCount = 1;
    }

    public void AttackSequence()
    {
        if (enemyAI.AtackCount < enemyAI.HardAtackNum)
        {
            enemyAI.AtackCount++;
            StartCoroutine(AttackEnumerator(NormalAttackSetting));
        }
        else
        {
            enemyAI.AtackCount = 1;
            StartCoroutine(AttackEnumerator(HardAttackSetting));
        }
    }

    public IEnumerator AttackEnumerator(AttackSettings attackSettings)
    {
        
        enemyAI.IsAttacking = true;
        enemyAI.Agent.isStopped = true;

        bool originalUpdateRotation = enemyAI.Agent.updateRotation;

        if (enemyAI.EnemyHealth.Health <= 0) yield break;
        if (attackSettings.clip != null && transform.TryGetComponent(out AudioSource source))
        {
            source.PlayOneShot(attackSettings.clip);
        }

        enemyAI.Agent.updateRotation = false;

        enemyAI.Animator.SetTrigger(attackSettings.AttackAnimationPropertyName);
        yield return new WaitForSeconds(attackSettings.AttackWindupTime);

        Vector3 hitboxOrigin = transform.position;

        hitboxOrigin += transform.forward * attackSettings.HitboxOffset.z;
        hitboxOrigin += transform.right * attackSettings.HitboxOffset.x;
        hitboxOrigin += transform.up * attackSettings.HitboxOffset.y;

        bool haveHitPlayer = StatcHitboxCreator.TryHitWithBoxHitbox(hitboxOrigin, attackSettings.HitboxSize, enemyAI.PlayerMask, attackSettings.Damage, true, transform.rotation, attackSettings.effect);

        if (haveHitPlayer)
        {
            Debug.Log("Hit");
        }

        yield return new WaitForSeconds(attackSettings.Duration - attackSettings.AttackWindupTime);
        enemyAI.Animator.SetTrigger("StayAnimForEnemy");

        if (enemyAI.Player != null)
        {
            Vector3 direction = (enemyAI.Player.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                while (Quaternion.Angle(transform.rotation, targetRotation) > 5f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyAI.RotationSpeed * Time.deltaTime);
                    yield return null;
                }
                transform.rotation = targetRotation;
            }
        }

        if (enemyAI.IsPlayerInTrigger && enemyAI.Player != null)
        {
            yield return new WaitForSeconds(attackSettings.Cooldown);
            enemyAI.IsAttacking = false;
            AttackSequence();
        }
        else
        {
            enemyAI.IsAttacking = false;
            enemyAI.Agent.updateRotation = true;
            enemyAI.Agent.isStopped = false;
        }
    }
}
