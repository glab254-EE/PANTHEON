using UnityEngine;

[System.Serializable]
public class AttackSettings
{
    [field:SerializeField]
    public string AttackAnimationPropertyName = "Attack1";
    [field: SerializeField]
    public int AttackAnimationPropertyIndex = 1;
    [field:SerializeField]
    public Vector3 HitboxOffset;
    [field:SerializeField]
    public Vector3 HitboxSize;
    [field:SerializeField]
    public double Damage = 1;
    [field:SerializeField]
    public float AttackWindupTime = 0f;
    [field:SerializeField]
    public float Duration = 1f;
    [field:SerializeField]
    public float Cooldown = 1f;
    [field: SerializeField]
    public ADamageEffect effect = null;
}
