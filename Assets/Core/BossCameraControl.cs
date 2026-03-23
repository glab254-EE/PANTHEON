using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BossCameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineCamera bossCamera;
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private int CameraPriorityDifference;
    [SerializeField] private float pointsRequiement = 50;
    [SerializeField] private float bossViewDuration;
    [SerializeField] private WillpowerManager willpowerManager;
    [SerializeField] private UnityEvent OnViewEvents;
    private int PlayerCameraPriority;
    private int BossCameraPriority;
    private void Start()
    {
        PlayerCameraPriority = playerCamera.Priority.Value;
        BossCameraPriority = bossCamera.Priority.Value;
        willpowerManager.TryConnectEvent(new(CheckPoints));
    }
    void CheckPoints(float currentPoints)
    {
        if (currentPoints >= pointsRequiement)
        {
            StartCoroutine(ShowBossArena());
        }
    }
    private IEnumerator ShowBossArena()
    {
        OnViewEvents?.Invoke();
        playerCamera.Priority.Value = PlayerCameraPriority-CameraPriorityDifference;
        bossCamera.Priority = BossCameraPriority + CameraPriorityDifference;

        yield return new WaitForSeconds(bossViewDuration);

        playerCamera.Priority = PlayerCameraPriority;
        bossCamera.Priority = BossCameraPriority;

        StopCoroutine(ShowBossArena());
    }
}
