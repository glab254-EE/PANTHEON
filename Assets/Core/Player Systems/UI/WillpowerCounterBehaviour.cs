using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class StatisticCounterBehaviour : MonoBehaviour
{
    [SerializeField]
    private PlayerStatSO targetStatisticSO;
    [SerializeField]
    private string TextPrefix = "WILLPOWER: ";
    [SerializeField]
    private bool DisableOnZeroValue = false;
    private TMP_Text textLabel;
    void Start()
    {
        textLabel = GetComponent<TMP_Text>();
        targetStatisticSO.OnUpdate += OnWillSmithUpdate;
        OnWillSmithUpdate(targetStatisticSO.CurrentValue);
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
