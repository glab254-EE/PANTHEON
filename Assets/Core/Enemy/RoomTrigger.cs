using UnityEngine;
using System.Collections.Generic;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private List<EnemyAI> enemiesInRoom;
    [SerializeField] private Transform enemy;
    [SerializeField] private bool isEnemy = true;

    private void Update()
    {
        if (isEnemy)
        {
            gameObject.transform.position = enemy.position;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var enemy in enemiesInRoom)
            {
                enemy.Activate(other.transform);

                /*if (!enemy.gameObject.activeInHierarchy)      //Edited
                {                                               //Edited
                    enemy.gameObject.SetActive(true);           //Edited
                }*/                                             //Edited
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isEnemy)
        {
            foreach (var enemy in enemiesInRoom)
            {
                enemy.DeActivate(other.transform);
            }
        }
    }
}