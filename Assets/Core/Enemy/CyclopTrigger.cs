using System.Collections.Generic;
using UnityEngine;

public class CyclopTrigger : MonoBehaviour
{
    [SerializeField] private CyclopAI cyclopAI;
    [SerializeField] private Transform enemy;

    private void Update()
    {
        gameObject.transform.position = enemy.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cyclopAI.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cyclopAI.DeActivate();
        }
    }
}
