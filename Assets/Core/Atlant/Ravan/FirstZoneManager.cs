using UnityEngine;

public class FirstZoneManager : MonoBehaviour
{
    [SerializeField] public GameObject atackZoneModel;

    public bool FirstZoneActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstZoneActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstZoneActive = false;
        }
    }
}
