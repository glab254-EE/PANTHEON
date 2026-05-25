using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class StatisticCounterBehaviour : MonoBehaviour
{
    [SerializeField]
    private PlayerStatSO targetStatisticSO;
    [SerializeField]
    private int TargetCurrencyIndex = -1;
    [SerializeField]
    private string TextPrefix = "WILLPOWER: ";
    [SerializeField]
    private bool DisableOnZeroValue = false;
    private TMP_Text textLabel;
    void Start()
    {
        textLabel = GetComponent<TMP_Text>();
        if (TargetCurrencyIndex >= 0 && TargetCurrencyIndex < PlayerStatisticsManager.Currencies.Count)
        {
            try
            {
                targetStatisticSO = PlayerStatisticsManager.Currencies[TargetCurrencyIndex];                
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }
        targetStatisticSO.OnUpdate += OnWillSmithUpdate;
        OnWillSmithUpdate(targetStatisticSO.CurrentValue);
    }
    void OnDestroy()
    {
        targetStatisticSO.OnUpdate -= OnWillSmithUpdate;        
    }
    private void OnWillSmithUpdate(double points)
    {
        if (DisableOnZeroValue == true)
        {
            gameObject.SetActive(points > 0);
        }
        textLabel.text = TextPrefix+points.ToString();   
    }
}
