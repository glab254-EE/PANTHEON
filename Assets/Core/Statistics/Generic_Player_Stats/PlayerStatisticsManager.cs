using System.Collections.Generic;

public static class PlayerStatisticsManager
{
    public static List<PlayerAttributeClass> AvailableAttributeData { get; private set; }
    public static List<PlayerStatSO> Currencies { get; private set; }
    public static void SetUp(List<PlayerAttributeClass> data, List<PlayerStatSO> currencies)
    {
        if (AvailableAttributeData != null || data == null || currencies == null)
        {
            return;
        }
        Currencies = currencies;
        AvailableAttributeData = data;
        for (int i = 0; i < AvailableAttributeData.Count; i++)
        {
            PlayerAttributeClass attribute = AvailableAttributeData[i];
            if (attribute.attribute != null)
            {
                attribute.attribute.CurrentValue = attribute.attribute.StartingValue;
            }
        }
    }
    public static bool TryGetValue(int index, out double value)
    {
        value = 0;
        if (index >= 0 && index < AvailableAttributeData.Count)
        {
            value = AvailableAttributeData[index].Value;
            return true;
        }
        return false;
    }
    public static bool TryGetValue(int index, out double value, out PlayerStatSO stat)
    {
        value = 1;
        stat = null;
        if (AvailableAttributeData==null)
        {
            return false;
        }
        if (index >= 0 && index < AvailableAttributeData.Count)
        {
            stat = AvailableAttributeData[index].attribute;
            value = AvailableAttributeData[index].Value;
            return true;
        }
        return false;
    }
    public static bool TryResetStatistics()
    {
        bool successfull = false;
        foreach (PlayerStatSO currency in Currencies)
        {
            if (currency.CurrentValue <= 0) continue;
            successfull = true;
            currency.CurrentValue = 0;
            currency.InvokeEvent();
        }
        return successfull;
    }
    public static bool TryUpgrade(int index)
    {
        if (index >= 0 && index < AvailableAttributeData.Count)
        {
            PlayerAttributeClass playerAttributeClass = AvailableAttributeData[index];
            if (playerAttributeClass == null || playerAttributeClass.CurrencyIndex < 0 || playerAttributeClass.CurrencyIndex >= Currencies.Count) return false;
            PlayerStatSO currency = Currencies[playerAttributeClass.CurrencyIndex];
            if (currency == null) return false;
            if (!playerAttributeClass.TryUpgrade(currency.CurrentValue,out double cost)) return false;
            currency.CurrentValue -= cost;
            currency.InvokeEvent();
            return true;
        }
        return false;
    }
}
