using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpecificOnlyDamageStrategy", menuName = "Scriptable Objects/EnemySpecificOnlyDamageStrategy")]
public class EnemySpecificOnlyDamageStrategy : AEnemyHealthStrategy
{
    [SerializeField]
    private List<ADamageEffect> EffectsToDamage;
    public override bool TryDamage(ADamageEffect incomingEffect, double ammount, out double output)
    {
        output = 0;
        if (EffectsToDamage.Contains(incomingEffect))
        {
            output = ammount;
            return true;
        }
        foreach (ADamageEffect effect in EffectsToDamage)
        {
            if (incomingEffect == effect || (incomingEffect != null && incomingEffect.GetType() == effect.GetType()))
            {
                output = ammount;
                return true;                
            }
        }
        return false;
    }
}
