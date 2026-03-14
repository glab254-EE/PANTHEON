using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Heavy Damage Effect", menuName = "SO/Heavy Damage Effect")]
public class HeavyDamageEffect : ADamageEffect
{
    public override Task DamageEffect(IDamagable damagable, double BaseDamage)
    {
        return Task.CompletedTask;
    }
}
