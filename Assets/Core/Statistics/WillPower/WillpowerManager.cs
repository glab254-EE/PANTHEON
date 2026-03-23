using UnityEngine;

public class WillpowerManager : MonoBehaviour
{
    [field:SerializeField]
    public float CurrentPoints { get; private set; } = 0;
    public delegate void onPointsUpdateBaseDelegade(float points);
    internal onPointsUpdateBaseDelegade PointsUpdateConncetions;
    void OnDestroy()
    {
        PointsUpdateConncetions = null;
    }
    public bool TryConnectEvent(onPointsUpdateBaseDelegade ToConnect)
    {
        try
        {
            if (PointsUpdateConncetions == null)
            {
                PointsUpdateConncetions = ToConnect;
            } else
            {
                PointsUpdateConncetions += ToConnect;
            }
        }
        catch
        {
            return false;
        }
        return true;
    }
    public bool TryAddPoints(WillOrbCollectionBehaviour orb)
    {
        if (orb == null || orb.gameObject == null || orb.Points <= 0) return false;

        CurrentPoints += orb.Points;
        PointsUpdateConncetions?.Invoke(CurrentPoints);
        if (orb.transform.root != orb.transform && orb.transform.root.gameObject != null)
        {
            Destroy(orb.transform.root.gameObject);
        } else
        {
            Destroy(orb.gameObject);
        }
        return true;        
    }
}
