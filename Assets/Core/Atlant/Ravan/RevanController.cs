using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class RevanController : MonoBehaviour
{
    [SerializeField] private FirstZoneManager firstZone;
    [SerializeField] private SecondZoneManager secondZone;
    [SerializeField] private ThirdZoneManager thirdZone;
    [SerializeField] private string DeathName = "Dead";
    [SerializeField] private string AppearedName = "Appeared";
    [SerializeField] private string AppearingName = "Appearing";
    [SerializeField] private string firstAtackName = "LeftAtack";
    [SerializeField] private string secondAtackName = "MidleAtack";
    [SerializeField] private string thirdAtackName = "RightAtack";
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float SecondPhaseTimer = 10f;
    [SerializeField] private AttackSettings AttackSetting;
    [SerializeField] private LayerMask playerMask;
    public bool CutsceneActive = true;
    public bool InSecondPhase = false;
    private bool CanAttack = true;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(SecondPhaseEnumerator());
    }

    public void Activate()
    {
        animator.SetTrigger(AppearingName);
        animator.SetBool(AppearedName,true);
    }
    public void Die()
    {
        animator.SetTrigger(DeathName);
        InSecondPhase = false;
        CanAttack = false;
    }
    public void RevanAttackSequence()
    {
        if (CutsceneActive || !CanAttack) return;

        if (firstZone.FirstZoneActive)
        {
            firstZone.atackZoneModel.SetActive(true);
            animator.SetTrigger(firstAtackName);
            return;
        }
        if (secondZone.SecondZoneActive)
        {
            secondZone.atackZoneModel.SetActive(true);
            animator.SetTrigger(secondAtackName);
            return;
        }
        if (thirdZone.ThirdZoneActive)
        {
            thirdZone.atackZoneModel.SetActive(true);
            animator.SetTrigger(thirdAtackName);
            return;
        }
    }

    private IEnumerator TimerAtackEnumerator()
    {
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(cooldown);

        RevanAttackSequence();
    }

    private IEnumerator SecondPhaseEnumerator()
    {
        while (CanAttack)
        {
            if (!InSecondPhase) 
            {
                yield return null;
                continue;
            }

            cooldown = 0;
            yield return new WaitForSeconds(SecondPhaseTimer);
            cooldown = 2;
            yield return new WaitForSeconds(SecondPhaseTimer);
        }
    }
    public void RevanAtack(int location)
    {
        Transform targetPos;
        switch (location)
        {
            case 1:
                targetPos = secondZone.transform;
                break;
            case 2:
                targetPos = thirdZone.transform;
                break;
            default:
                targetPos = firstZone.transform;
                break;
        }
        Vector3 AttackPosition = targetPos.position;
        AttackPosition += targetPos.right*AttackSetting.HitboxOffset.x;
        AttackPosition += targetPos.up*AttackSetting.HitboxOffset.y;
        AttackPosition += targetPos.forward*AttackSetting.HitboxOffset.z;
        bool hit = StatcHitboxCreator.TryHitWithBoxHitbox(AttackPosition,AttackSetting.HitboxSize,playerMask,AttackSetting.Damage,null,false,targetPos.rotation,AttackSetting.effect);
        firstZone.atackZoneModel.SetActive(false);
        secondZone.atackZoneModel.SetActive(false);
        thirdZone.atackZoneModel.SetActive(false);
    }
}