using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class BossCameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineCamera bossCamera;
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private float needPoint = 50;
    [SerializeField] private float bossViewDuration;

    [SerializeField] private float playerPoint;

    private void Start()
    {
        playerCamera.Priority = 20;
        bossCamera.Priority = 10;
    }

    private void Update()
    {
        if (playerPoint >= needPoint)
        {
            StartCoroutine(ShowBossArena());
        }
    }

    IEnumerator ShowBossArena()
    {
        playerCamera.Priority = 10;
        bossCamera.Priority = 20;

        yield return new WaitForSeconds(bossViewDuration);

        playerCamera.Priority = 20;
        bossCamera.Priority = 10;

        StopCoroutine(ShowBossArena());
    }
}
