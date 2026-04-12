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

    private Animator animator;

    public bool CutsceneActive = true;

    //[field: SerializeField] private AttackSettings firstAtack;
    //[field: SerializeField] private AttackSettings secondAtack;
    //[field: SerializeField] private AttackSettings thirdAtackж

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RevanAttackSequence()
    {
        if (CutsceneActive) { return; }

        Debug.Log("Поехали");
        if (firstZone.FirstZoneActive)
        {
            firstZone.atackZoneModel.SetActive(true);
            StarterAnimation(firstAtackName);
            return;
        }
        if (secondZone.SecondZoneActive)
        {
            secondZone.atackZoneModel.SetActive(true);
            StarterAnimation(secondAtackName);
            return;
        }
        if (thirdZone.ThirdZoneActive)
        {
            thirdZone.atackZoneModel.SetActive(true);
            StarterAnimation(thirdAtackName);
            return;
        }
    }

    private void StarterAnimation(string nameAnim)
    {
        animator.SetTrigger(nameAnim);
    }

    public void RevanAtack()
    {
        firstZone.atackZoneModel.SetActive(false);
        secondZone.atackZoneModel.SetActive(false);
        thirdZone.atackZoneModel.SetActive(false);
        //Выщитывается хитбокс + получение урона
        Debug.Log("Удар!!!");
    }

    public void IdleAnimation()
    {
        animator.SetTrigger("Idle");
        Debug.Log("Айдл");
    }

    /*private IEnumerator RevanAtackEnumerator (AttackSettings attackSettings)
    {
        yield return new WaitForSeconds(attackSettings.Cooldown);

        firstZone.atackZoneModel.SetActive(false);
        secondZone.atackZoneModel.SetActive(false);
        thirdZone.atackZoneModel.SetActive(false);

        animator.SetTrigger(attackSettings.AttackAnimationPropertyName);

        yield return new WaitForSeconds(attackSettings.AttackWindupTime);

        //Выщитывается хитбокс + получение урона

        yield return new WaitForSeconds(attackSettings.Duration - attackSettings.AttackWindupTime);

        animator.SetTrigger("Idle");

        RevanAtack();
    }*/

}
