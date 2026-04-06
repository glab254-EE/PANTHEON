using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraRollBehaviour : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera Camera;
    [SerializeField]
    private float Speed = 1;
    [SerializeField]
    private float MaximumRoll = 1;
    [SerializeField]
    private float TimeForDirectionChange = 1;
    public bool IsEnabled = false;
    float currentValue = 0;
    float currentDirection = 0;
    void Start()
    {
        currentDirection = Speed;
    }
    void Update()
    {
        if (!IsEnabled || Camera == null) return;
        currentValue+=currentDirection*Speed*Time.deltaTime;
        if (Mathf.Abs(currentValue) > MaximumRoll)
        {
            currentValue = MaximumRoll * Mathf.Sign(currentDirection);
            if (currentDirection != Speed * Mathf.Sign(currentDirection))
            {
                float sig = Mathf.Sign(currentDirection);
                currentDirection = Speed*sig;
            }
            StartCoroutine(ChangeDirectionEnumerator(-currentDirection));
        }
        Camera.Lens.Dutch = currentValue;
    }
    IEnumerator ChangeDirectionEnumerator(float dir)
    {
        currentDirection = 0;
        yield return new WaitForSeconds(TimeForDirectionChange);
        currentDirection = dir;
    }
}
