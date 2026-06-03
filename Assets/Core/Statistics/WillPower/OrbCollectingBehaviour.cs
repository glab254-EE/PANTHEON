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
    [field:SerializeField]
    private int TargetCurrencyIndex = -1;
    public CollectWillpowerToActivateHandler manager;
    private bool collected = false;
    private AudioSource source;
    void OnTriggerStay(Collider other)
    {
        if (other == null || other.gameObject == null || collected) return;
        if (TargetCurrencyIndex != -1 && PlayerStatisticsManager.Currencies.Count > TargetCurrencyIndex)
        {
            TargetCurrencyStatistic = PlayerStatisticsManager.Currencies[TargetCurrencyIndex];
        }
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
                TargetCurrencyStatistic?.InvokeEvent();
            }
            if (collected == true)
            {
                if (gameObject.TryGetComponent(out source) && source.clip != null)
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
    void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null || collected) return;
        if (TargetCurrencyIndex != -1 && PlayerStatisticsManager.Currencies.Count > TargetCurrencyIndex)
        {
            TargetCurrencyStatistic = PlayerStatisticsManager.Currencies[TargetCurrencyIndex];
        }
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
                TargetCurrencyStatistic?.InvokeEvent();
            }
            if (collected)
            {
                if (gameObject.TryGetComponent(out source))
                {
                    gameObject.SetActive(true);
                    StartCoroutine(DestroyEnumerator());
                }
                else
                {
                    Destroy(transform.root.gameObject);
                }
            }
        }
    }
    IEnumerator DestroyEnumerator()
    {
        if (gameObject.TryGetComponent(out MeshRenderer renderer))
        {
            renderer.enabled = false;
            
        }
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        Destroy(transform.root.gameObject);
    }
}
