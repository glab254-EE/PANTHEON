using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "Scriptable Objects/PlayerStatSO")]
public class PlayerStatSO : ScriptableObject
{
    [field: SerializeField]
    public string DisplayName { get; private set; } = "";
    [field: SerializeField]
    public double StartingValue { get; private set; } = 1f;
    public double CurrentValue = 1f;
    public event Action<double> OnUpdate;
    internal void InvokeEvent()
    {
        if (OnUpdate == null) return;
        OnUpdate?.Invoke(CurrentValue);
    }
    private void OnEnable()
    {
        CurrentValue = StartingValue;
        InvokeEvent();
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        InvokeEvent();
    }
#endif
}
