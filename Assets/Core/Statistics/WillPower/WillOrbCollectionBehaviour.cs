using UnityEngine;

public class WillOrbCollectionBehaviour : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerlayer;
    [field:SerializeField]
    public float Points {get;private set;}
    public WillpowerManager manager;
    private bool collected = false;
    void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null || collected) return;
        if (LayerMaskUtils.CompareCollisionlayers(other.gameObject.layer,playerlayer))
        {
            if (manager.TryAddPoints(this) && gameObject != null)
            {
                collected = true;
                Destroy(gameObject);
            }
        }
    }
}
