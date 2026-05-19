using System.Collections;
using UnityEngine;

public class RevanController : MonoBehaviour
{
    [SerializeField] private FirstZoneManager firstZone;
    [SerializeField] private SecondZoneManager secondZone;
    [SerializeField] private ThirdZoneManager thirdZone;
    [SerializeField] private string firstAtackName = "LeftAtack";
    [SerializeField] private string secondAtackName = "MidleAtack";
    [SerializeField] private string thirdAtackName = "RightAtack";
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float SecondPhaseTimer = 10f;

    public bool CutsceneActive = true;

    public bool InSecondPhase = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        RevanAttackSequence();
        StartCoroutine(SecondPhaseEnumerator());
    }

    public void RevanAttackSequence()
    {
        if (CutsceneActive) { return; }

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
        while (true)
        {
            if (!InSecondPhase) { continue; }
            Debug.Log("Second");

            cooldown = 0;
            yield return new WaitForSeconds(SecondPhaseTimer);
            cooldown = 2;
            yield return new WaitForSeconds(SecondPhaseTimer);
        }
    }

    public void RevanAtack()
    {
        firstZone.atackZoneModel.SetActive(false);
        secondZone.atackZoneModel.SetActive(false);
        thirdZone.atackZoneModel.SetActive(false);
        //Выщитывается хитбокс + получение урона
    }
}