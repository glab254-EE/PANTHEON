using UnityEngine;

public class WillOrbCollectionBehaviour : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerlayer;
    [field:SerializeField]
    public float Points {get;private set;}
    public WillpowerManager manager;
    void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null) return;
        if (LayerMaskUtils.CompareCollisionlayers(other.gameObject.layer,playerlayer))
        {
            if (manager.TryAddPoints(this) && gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
