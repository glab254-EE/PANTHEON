using UnityEngine;

public class SecondZoneManager : MonoBehaviour
{
    [SerializeField] public GameObject atackZoneModel;

    public bool SecondZoneActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SecondZoneActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SecondZoneActive = false;
        }
    }
}
