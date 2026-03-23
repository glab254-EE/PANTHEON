using System.Collections.Generic;
using UnityEngine;

public class SpawnOrbOnDisable : MonoBehaviour
{
    [SerializeField]
    private WillpowerManager CallYourManagerNow; // american karen joke.
    [SerializeField]
    private GameObject prefab;
    public void SpawnObject()
    {
        GameObject cloned = Instantiate(prefab,transform.position,Quaternion.identity);
        WillOrbCollectionBehaviour behaviour = cloned.GetComponentInChildren<WillOrbCollectionBehaviour>();
        if (behaviour != null)
        {
            behaviour.manager = CallYourManagerNow;
        }
    }
}
