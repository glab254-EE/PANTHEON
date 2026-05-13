using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatisticPanelHandler : MonoBehaviour
{
    [SerializeField]
    private string NamingTextFormat = "{0}\n{1} -> {2}";
    [SerializeField]
    private string ButtonTextFormat = "Upgrade for {0} willpower";
    [SerializeField]
    private string MaxLevelString = "MAX LEVEL";
    [SerializeField]
    private TMP_Text NamingText;
    [SerializeField]
    private TMP_Text ButtonTexet;
    [SerializeField]
    private Button UpgradeButton;
    private int statIndex = 0;
    public void Initialize(int TargetIndex)
    {
        statIndex = TargetIndex;
        UpdateText();
        UpgradeButton.onClick.AddListener(OnButtonClick);
    }
    void OnButtonClick()
    {
        if (PlayerStatisticsManager.TryUpgrade(statIndex))
        {
            UpdateText();
        }
    }
    void UpdateText()
    {
        PlayerAttributeClass attributeClass = PlayerStatisticsManager.AvailableAttributeData[statIndex];
        if (attributeClass != null)
        {
            double nextCost = attributeClass.NextCost;
            double nextVal = attributeClass.NextValue;
            NamingText.text = string.Format(NamingTextFormat, attributeClass.attribute.DisplayName, attributeClass.Value, nextVal == -1 ? MaxLevelString : nextVal);
            ButtonTexet.text = string.Format(ButtonTextFormat, nextCost == -1 ? MaxLevelString : nextCost);
        }
    }
}
