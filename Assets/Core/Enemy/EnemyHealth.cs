using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [field: SerializeField] private Image HealthImage;
    [SerializeField] private float destroyDelay = 3;

    [field: SerializeField]
    private double MaxHealth;
    internal double Health { get; private set; }
    internal event Action<double> OnDamaged;
    private Animator _animator;
    private Collider col;

    void Start()
    {
        Health = MaxHealth;
        HealthImage.fillAmount = (float)(Health / MaxHealth);

        col = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
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
        if (_animator != null)
        {
            _animator.SetTrigger("EnemyDeath");
        }

        if (col != null) col.enabled = false;

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
