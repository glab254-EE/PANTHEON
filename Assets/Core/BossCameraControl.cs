using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BossCameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineCamera bossCamera;
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private float needPoint;
    [SerializeField] private float bossViewDuration;

    private void Start()
    {
        playerCamera.Priority = 20;
        bossCamera.Priority = 10;
    }

    IEnumerator ShowBossArena()
    {
        playerCamera.Priority = 10;
        bossCamera.Priority = 20;

        yield return new WaitForSeconds(bossViewDuration);

        playerCamera.Priority = 20;
        bossCamera.Priority = 10;

        StopCoroutine("ShowBossArena");
    }

    /*public void StopBossViewCoroutine()
    {
        StopCoroutine("ShowBossArena");
    }*/
}
