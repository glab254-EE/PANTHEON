using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [field: SerializeField] private Image HealthImage;
    [SerializeField] private float deathDuration = 3;
    [SerializeField] private EnemyAI enemyAI;

    [field: SerializeField]
    private double MaxHealth;
    public double Health; /*{ get; private set; }*/
    internal event Action<double> OnDamaged;
    //private Animator _animator;
    private Collider col;

    private void Update()
    {
        if (Health <= 0)
        {
            Die();
        }
    }

    void Start()
    {
        Health = MaxHealth;
        HealthImage.fillAmount = (float)(Health / MaxHealth);

        col = GetComponent<Collider>();
    }
    public bool TryDamage(double damage, ADamageEffect effect)
    {
        if (Health <= 0)
        {
            Die();
            return false;
        }
        Health -= damage;

        /*if (effect != null) 
        {
            effect.DamageEffect(this, damage);
        }*/

        HealthImage.fillAmount = (float)(Health / MaxHealth);

        OnDamaged?.Invoke(Health);

        Debug.Log(Health);

        return true;
    }

    private void Die()
    {
        enemyAI.Animator.SetTrigger("EnemyDeath");

        /*if (enemyAI.Animator != null)
        {
            enemyAI.Animator.SetTrigger("EnemyDeath");
        }*/

        //if (col != null) col.enabled = false;

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(deathDuration);
        gameObject.SetActive(false);
    }
}
