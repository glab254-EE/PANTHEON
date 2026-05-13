using UnityEngine;

public class UpgradeScreenHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject template;
    [SerializeField]
    private Transform parent; 
    void Start()
    {
        for (int i = 0; i < PlayerStatisticsManager.AvailableAttributeData.Count; i++)
        {
            GameObject cloned = Instantiate(template, parent);
            if (cloned.TryGetComponent(out StatisticPanelHandler panelHandler))
            {
                panelHandler.Initialize(i);
            }
        }
    }
}
