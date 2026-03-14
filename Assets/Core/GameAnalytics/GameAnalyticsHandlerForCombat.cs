using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsHandler : MonoBehaviour
{
    void Start()
    {
        GameAnalytics.Initialize();
    }
    public void OnAction(string message)
    {
        if (message == "death")
        {
            GameAnalytics.NewDesignEvent("Deaths");
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Arena01");
        } else if(message == "light attack")
        {
            GameAnalytics.NewDesignEvent("Light attacks");
        } else if (message == "heavy attack")
        {
            GameAnalytics.NewDesignEvent("Heavy attacks");
        } else if ( message == "roll")
        {
            GameAnalytics.NewDesignEvent("Rolled");
        }
        else 
        {
            GameAnalytics.NewDesignEvent(message);
        }
    }
}
