using UnityEngine;

public class AEnemyHealthStrategy : ScriptableObject
{
    public virtual bool TryDamage(ADamageEffect incomingEffect, double ammount, out double output)
    {
        output = ammount;
        return true;
    }
}
