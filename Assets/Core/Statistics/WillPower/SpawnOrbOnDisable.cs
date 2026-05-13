using System.Collections.Generic;
using UnityEngine;

public class SpawnOrbOnDisable : MonoBehaviour
{
    [SerializeField]
    private EnemyHealth enemyHealth;
    [SerializeField]
    private CollectWillpowerToActivateHandler CallYourManagerNow; // american karen joke.
    [SerializeField]
    private GameObject prefab;
    void Start()
    {
        enemyHealth.OnDamaged += OnDamaged;   
    }
    private void OnDamaged(double current)
    {
        if (current <= 0)
        {
            SpawnObject();
        }
    }
    public void SpawnObject()
    {
        GameObject cloned = Instantiate(prefab,transform.position,Quaternion.identity);
        OrbCollectingBehaviour behaviour = cloned.GetComponentInChildren<OrbCollectingBehaviour>();
        if (behaviour != null && CallYourManagerNow != null)
        {
            behaviour.manager = CallYourManagerNow;
        }
    }
}
