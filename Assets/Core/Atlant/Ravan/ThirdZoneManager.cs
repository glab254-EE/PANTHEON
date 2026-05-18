using UnityEngine;

public class ThirdZoneManager : MonoBehaviour
{
    [SerializeField] public GameObject atackZoneModel;

    public bool ThirdZoneActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThirdZoneActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThirdZoneActive = false;
        }
    }
}
