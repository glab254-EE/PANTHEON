using UnityEngine;

public abstract class APlayerDamageTakeStrategy : ScriptableObject
{
    public virtual double GetIncomingDamageFromBeingHit(double incoming, GameObject playerObject, GameObject source = null)
    {
        return incoming;
    }
}
