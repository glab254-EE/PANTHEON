using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthHandler : MonoBehaviour, IDamagable
{
    [SerializeField] private Image HealthImage; //Edited

    [field:SerializeField]
    private double MaxHealth = 10;
    [field:SerializeField]
    private int GoddedTimerMilisec = 100;
    [field: SerializeField]
    public double DamageMultiplier { get; internal set; } = 1;
    internal bool Godded {get;private set;}
    internal double Health {get;private set;}
    internal event Action<double> OnDamaged;
    void Awake()
    {
        Health = MaxHealth;

        if (HealthImage != null)
        {
            HealthImage.fillAmount = (float)(Health / MaxHealth); //Edited
        }
    }
    public bool TryDamage(double damage, ADamageEffect e)
    {
        if (Godded || Health <= 0)
        {
            return false;
        }

        Health -= damage * DamageMultiplier;
        Health = Math.Clamp(Health, 0, MaxHealth);

        if (e != null)
        {
            e.DamageEffect(this, damage);
        }

        if (HealthImage != null)
        {
            HealthImage.fillAmount = (float)(Health / MaxHealth); //Edited
        }

        if (damage > 0)
        {
            Godded = true;
            Task.Run(GoddedTask);
        }
        OnDamaged.Invoke(Health);
#if UNITY_EDITOR
        Debug.Log(Health);
        #endif
        return true;
    }
    private async Task GoddedTask()
    {
        Godded = true;
        await Task.Delay(GoddedTimerMilisec);
        Godded = false;
    }
}
