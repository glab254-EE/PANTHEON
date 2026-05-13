using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerAttributeClass
{
    [Serializable]
    public struct PlayerAttirbuteLevelingDataField
    {
        [field: SerializeField]
        public double Value { get; private set; }
        [field: SerializeField]
        public double Cost { get; private set; }
    }
    [field: SerializeField]
    public List<PlayerAttirbuteLevelingDataField> levelingData { get; private set; }
    [field: SerializeField]
    public PlayerStatSO attribute { get; private set; }
    [field: SerializeField]
    public int CurrencyIndex { get; private set; } = 0;
    [field: SerializeField]
    public int CurrentLevelIndex { get; private set; } = 0;
    public double NextCost
    {
        get
        {
            if (levelingData.Count <= CurrentLevelIndex + 1) return -1;
            PlayerAttirbuteLevelingDataField field = levelingData[CurrentLevelIndex + 1];
            return field.Cost;
        }
    }
    public double NextValue
    {
        get
        {
            if (levelingData.Count <= CurrentLevelIndex + 1) return -1;
            PlayerAttirbuteLevelingDataField field = levelingData[CurrentLevelIndex + 1];
            return field.Value;
        }
    }
    public bool TryUpgrade(double current, out double cost)
    {
        cost = 0;
        if (CurrentLevelIndex < 0 || CurrentLevelIndex >= levelingData.Count)
        {
            return false;
        }
        if (NextCost > current)
        {
            return false;
        }
        cost = NextCost;
        attribute.CurrentValue = NextValue;
        attribute.InvokeEvent();
        CurrentLevelIndex++;
        return true;
    }
    public double Value => attribute.CurrentValue;
}
