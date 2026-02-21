using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [field:SerializeField]
    private DamagableContainer damagable;
    public bool OnHit(double damage,ADamageEffect effect = null)
    {
        return damagable.damagable.TryDamage(damage,effect);
    }
}
[System.Serializable]
struct DamagableContainer
{
    [field:SerializeField]
    private UnityEngine.Object Object;
    public IDamagable damagable
    {
        get
        {
            return (IDamagable) Object;
        }
    }
}