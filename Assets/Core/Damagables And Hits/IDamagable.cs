using UnityEngine;

public interface IDamagable
{
    public bool TryDamage(double damage, ADamageEffect effect, GameObject source);
}
