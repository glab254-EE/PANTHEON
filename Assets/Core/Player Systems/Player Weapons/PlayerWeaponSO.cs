using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponSO", menuName = "Scriptable Objects/PlayerWeaponSO")]
public class PlayerWeaponSO : ScriptableObject
{
    [SerializeField]
    public GameObject RightArmModel;
    [SerializeField]
    public GameObject LeftArmModel;
    [SerializeField]
    public RuntimeAnimatorController animator;
    [SerializeField]
    public float ComboDuration = 4f;
    [SerializeField]
    public List<AttackPattern> AttackCombos;
    [ SerializeField]
    public List<AttackPattern> HeavyAttackCombos;
}
