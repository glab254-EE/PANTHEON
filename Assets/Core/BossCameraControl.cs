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
    [SerializeField] private PlayerStatSO playerStat;
    [SerializeField] private float bossViewDuration;
    [SerializeField] private CollectWillpowerToActivateHandler willpowerManager;
    [SerializeField] private UnityEvent OnViewEvents;
    private int PlayerCameraPriority;
    private int BossCameraPriority;
    private bool shown = false;
    private void Start()
    {
        PlayerCameraPriority = playerCamera.Priority.Value;
        BossCameraPriority = bossCamera.Priority.Value;
        willpowerManager.TryConnectEvent(new(CheckPoints));
        if (playerStat != null)
        {
            playerStat.OnUpdate += new System.Action<double>(CheckPoints);
        }
    }
    void CheckPoints(float currentPoints)
    {
        if (currentPoints >= pointsRequiement && !shown)
        {
            shown = true;
            StartCoroutine(ShowBossArena());
        }
    }
    void CheckPoints(double currentPoints)
    {
        if (currentPoints >= pointsRequiement && !shown)
        {
            shown = true;
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
