using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class WillpowerCounterBehaviour : MonoBehaviour
{
    [SerializeField]
    private WillpowerManager willpowerManager;
    [SerializeField]
    private string TextPrefix = "ВОЛЯ: ";
    private TMP_Text textLabel;
    void Start()
    {
        textLabel = GetComponent<TMP_Text>();
        willpowerManager.TryConnectEvent(OnWillSmithUpdate);
    }
    private void OnWillSmithUpdate(float points)
    {
        textLabel.text = TextPrefix+points.ToString();   
    }
}
