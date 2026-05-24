using System.Collections;
using UnityEngine;

public class OrbCollectingBehaviour : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerlayer;
    [SerializeField]
    private PlayerStatSO TargetCurrencyStatistic;
    [field:SerializeField]
    public float Points {get;private set; }
    public CollectWillpowerToActivateHandler manager;
    private bool collected = false;
    private AudioSource source;
    void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null || collected) return;
        if (LayerMaskUtils.CompareCollisionlayers(other.gameObject.layer,playerlayer))
        {
            if (manager != null && manager.TryAddPoints(this) && gameObject != null)
            {
                collected = true;
            }
            if (TargetCurrencyStatistic != null)
            {
                collected = true;
                TargetCurrencyStatistic.CurrentValue += Points;
                TargetCurrencyStatistic.InvokeEvent();
            }
            if (collected)
            {
                if (gameObject.TryGetComponent(out source))
                {
                    StartCoroutine(DestroyEnumerator());
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    IEnumerator DestroyEnumerator()
    {
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        Destroy(gameObject);
    }
}
