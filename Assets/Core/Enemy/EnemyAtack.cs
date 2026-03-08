using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAtack : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;

    public IEnumerator AttackSequence()
    {
        enemyAI.IsAttacking = true;
        enemyAI.Agent.isStopped = true;

        bool originalUpdateRotation = enemyAI.Agent.updateRotation;

        if (enemyAI.EnemyHealth.Health <= 0) yield break;

        enemyAI.Agent.updateRotation = false;

        /*if (enemyAI.AtackCount <= enemyAI.HardAtackNum)
        {
            enemyAI.Animator.SetTrigger("EnemyAtack");
            yield return new WaitForSeconds(enemyAI.AttackSetting.AttackWindupTime);
        }
        else
        {
            enemyAI.Animator.SetTrigger("HardEnemyAtack");
            yield return new WaitForSeconds(enemyAI.AttackSetting.AttackWindupTime);
        }*/

        enemyAI.Animator.SetTrigger("EnemyAtack");
        yield return new WaitForSeconds(enemyAI.AttackSetting.AttackWindupTime);

        Vector3 hitboxOrigin = transform.position;

        hitboxOrigin += transform.forward * enemyAI.AttackSetting.HitboxOffset.z;
        hitboxOrigin += transform.right * enemyAI.AttackSetting.HitboxOffset.x;
        hitboxOrigin += transform.up * enemyAI.AttackSetting.HitboxOffset.y;

        bool haveHitPlayer = StatcHitboxCreator.TryHitWithBoxHitbox(hitboxOrigin, enemyAI.AttackSetting.HitboxSize, enemyAI.PlayerMask, enemyAI.AttackSetting.Damage, true, transform.rotation);

        if (haveHitPlayer)
        {
            Debug.Log("Hit");
        }

        yield return new WaitForSeconds(enemyAI.AttackSetting.Duration);
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
            yield return new WaitForSeconds(enemyAI.AttackSetting.Cooldown);
            enemyAI.IsAttacking = false;
            StartCoroutine(AttackSequence());
        }
        else
        {
            enemyAI.IsAttacking = false;
            enemyAI.Agent.updateRotation = true;
            enemyAI.Agent.isStopped = false;
        }
    }
}
