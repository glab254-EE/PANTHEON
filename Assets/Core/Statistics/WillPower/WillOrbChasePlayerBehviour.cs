using System.Transactions;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class WillOrbChasePlayerBehviour : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerMask;
    [SerializeField]
    private float TargetMaxSpeed = 2;
    [SerializeField]
    private float Acceloration = 4;
    [SerializeField]
    private Vector3 positionDifferenceRelativeToPlayer = Vector3.up;
    private Transform PlayerTransform = null;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (PlayerTransform == null)
        {
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            Vector3 target = PlayerTransform.position + PlayerTransform.up * positionDifferenceRelativeToPlayer.y + PlayerTransform.forward * positionDifferenceRelativeToPlayer.z + PlayerTransform.right * positionDifferenceRelativeToPlayer.x;
            Vector3 Direction = (target-transform.position).normalized;
            Vector3 lerped = Vector3.Lerp(rb.linearVelocity, Direction * TargetMaxSpeed, Time.fixedDeltaTime * Acceloration);
            rb.linearVelocity = lerped;
        }
    }
    private void OnPlayerFoundOrLost(Transform player)
    {
        PlayerTransform = player;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null) return;

        if (LayerMaskUtils.CompareCollisionlayers(other.gameObject.layer,playerMask))
        {
            OnPlayerFoundOrLost(other.transform);            
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != null && other.transform != null && PlayerTransform != null && PlayerTransform == other.transform)
        {
            OnPlayerFoundOrLost(null);
        }        
    }
}
