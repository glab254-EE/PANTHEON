using System.Threading.Tasks;
using UnityEngine;

public abstract class ADamageEffect : ScriptableObject
{
    public abstract Task DamageEffect(IDamagable damagable, double BaseDamage);
}
