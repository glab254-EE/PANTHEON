using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private Animator animator;
    [SerializeField] private string ImpactTriggerName = "Damaged";
    [field: SerializeField] private Image HealthImage;
    [SerializeField] private float deathDuration = 3;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private SpawnOrbOnDisable orb;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject bossDeathUIPanel;
    [SerializeField] private float bossDeathUIPanelDuration = 3;
    [SerializeField]private GameObject artifactUIPanel;
    [SerializeField] private float artifactUIPanelDuration = 10;

    public bool IsBoss;

    [field: SerializeField]
    private double MaxHealth;
    [SerializeField]
    private AEnemyHealthStrategy strategy;
    internal double Health { get; private set; }
    internal event Action<double> OnDamaged;
    private Collider col;

    void Awake()
    {
        Health = MaxHealth;
        HealthImage.fillAmount = (float)(Health / MaxHealth);

        col = GetComponent<Collider>();
    }
    public bool TryDamage(double damage, ADamageEffect effect,GameObject s)
    {
        if (Health <= 0)
        {
            Die();
            return false;
        }
        if (strategy != null)
        {
            if (!strategy.TryDamage(effect,damage, out damage))
            {
                return false;
            }
        }
        Health -= damage;
        if (Health <= 0)
        {
            Die();
            if (orb != null)
            {
                orb.SpawnObject();
            }
        }

        if (Health > 0)
        {
            if (animator != null && ImpactTriggerName != "") animator.SetTrigger(ImpactTriggerName);
        }
        /*if (effect != null) 
        {
            effect.DamageEffect(this, damage);
        }*/

        HealthImage.fillAmount = (float)(Health / MaxHealth);

        OnDamaged?.Invoke(Health);

        return true;
    }

    private void Die()
    {
        enemyAI.Animator.SetTrigger("EnemyDeath");

        if (col != null) col.enabled = false;

        if (IsBoss)
        {
            StartCoroutine(BossUI());
            return;
        }

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(deathDuration);
        StopCoroutine(DestroyAfterDelay());
        enemy.SetActive(false);
    }

    private IEnumerator BossUI()
    {
        bossDeathUIPanel.SetActive(true);

        yield return new WaitForSeconds(bossDeathUIPanelDuration);

        bossDeathUIPanel.SetActive(false);
        artifactUIPanel.SetActive(true);

        yield return new WaitForSeconds(artifactUIPanelDuration);

        artifactUIPanel.SetActive(false);
        StopCoroutine(BossUI());
        enemy.SetActive(false);
    }
}
