using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [System.Serializable]
    public struct DamagableContainer
    {
        [field: SerializeField]
        private UnityEngine.Object Object;
        public IDamagable damagable
        {
            get
            {
                return (IDamagable)Object;
            }
        }
    }
    [System.Serializable]
    public struct SpecialADamagableOvverideElement
    {
        [field: SerializeField]
        public AudioClip clip;
        [field: SerializeField]
        public ADamageEffect Index;
        public override readonly bool Equals(object obj)
        {
            return Object.Equals(Index, obj);
        }
        public override readonly int GetHashCode()
        {
            return Index.GetHashCode();
        }
    }
    [field:SerializeField]
    private DamagableContainer damagable;
    [field: SerializeField]
    private AudioClip HitClip;
    [field: SerializeField]
    private List<SpecialADamagableOvverideElement> SpecialClipsOvveride;
    public bool OnHit(double damage,GameObject HitSource,ADamageEffect effect = null)
    {
        AudioClip toPlayClip = HitClip;
        if (SpecialClipsOvveride.Count > 0 && effect != null)
        {
            foreach(SpecialADamagableOvverideElement element in SpecialClipsOvveride)
            {
                if (element.Equals(effect) || (effect != null && element.Index.GetType() == effect.GetType()))
                {
                    toPlayClip = element.clip;
                    break;
                }
            }
        }
        if (toPlayClip != null && gameObject.TryGetComponent(out AudioSource source))
        {
            source.PlayOneShot(toPlayClip);
        }
        return damagable.damagable.TryDamage(damage,effect,HitSource);
    }
}