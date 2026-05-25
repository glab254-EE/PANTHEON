using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Holy Shield Strategy", menuName = "Scriptable Objects/Player Damage Strategies/Holy Shit Strategy of blocking enemies")]
public class HolyShieldBlockingStrategy : APlayerDamageTakeStrategy
{
    [Serializable]
    struct HolyShieldStrategyParamsOverrideForDamagable // jesus, this is shit name
    {
        [SerializeField]
        public ADamageEffect effect;
        [SerializeField]
        public HolyShieldBlockingStrategyParametres overide;
        public override bool Equals(object obj)
        {
            return effect.Equals(obj);
        }
        public override int GetHashCode()
        {
            return effect.GetHashCode();
        }
    }
    [Serializable]
    struct HolyShieldBlockingStrategyParametres
    {
        [SerializeField]
        public double DamageReduction;
        [SerializeField]
        public float StaminaReduction;
        [SerializeField]
        public float KnockbackMultiplier;
        [SerializeField]
        public float StunDuration;
    }
    [SerializeField]
    private List<ADamageEffect> EffectsWhatCanHit;
    [SerializeField]
    private HolyShieldBlockingStrategyParametres baseParams;
    [SerializeField]
    private List<HolyShieldStrategyParamsOverrideForDamagable> HolyShieldStrategyParamsOverrideForDamagables;
    public override double GetIncomingDamageFromBeingHit(double incoming, GameObject playerObject, GameObject source = null, ADamageEffect effect = null)
    {
        HolyShieldBlockingStrategyParametres pararr = baseParams;
        foreach (HolyShieldStrategyParamsOverrideForDamagable holyShieldStrategyParamsOverrideForDamagable in HolyShieldStrategyParamsOverrideForDamagables)
        {
            if (holyShieldStrategyParamsOverrideForDamagable.Equals(effect))
            {
                pararr = holyShieldStrategyParamsOverrideForDamagable.overide;
                break;
            }
        }
        if (playerObject.TryGetComponent(out StaminaBehaviour behaviour) &&!EffectsWhatCanHit.Contains(effect))
        {
            if (behaviour.TryTakeStamina(pararr.StaminaReduction))
            {
                if (source != null && source.TryGetComponent(out Rigidbody rb))
                {
                    if (source.TryGetComponent(out EnemyAI ai)) ai.StunnedTime = pararr.StunDuration;
                    
                    Vector3 velocity = (source.transform.position- playerObject.transform.position).normalized * pararr.KnockbackMultiplier;
                    rb.AddForce(velocity, ForceMode.Impulse);
                }

                return incoming / pararr.DamageReduction;
            }
        }
        return incoming;
    }
}
