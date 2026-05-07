using System.Collections;
using TMPro;
using UnityEngine;

public class DialogWindowController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField]
    private TMP_Text TextField;
    [SerializeField]
    private CanvasGroup DialogWindowCanvas;
    [Header("Fade Settings")]
    [SerializeField]
    private float AppearTime = 0.5f;
    [SerializeField]
    private float DissapearTime = 1f;
    [SerializeField]
    private float TransitionStepCooldown = 0.05f;
    [Header("Typewritter Settings")]
    [SerializeField]
    private float NormalLetterTypingDelay = 0.1f;
    [SerializeField]
    private float PunctuiationTypingDelay = 0.3f;
    private Coroutine currentCoroutine;
    private bool IsDialogEnabled
    {
        get
        {
            if (DialogWindowCanvas != null)
            {
                return DialogWindowCanvas.gameObject.activeInHierarchy;
            }
            return false;
        }
    }
    private Awaitable TextShowEnumerator(string TargetText, ) 
    { 
    }
    private IEnumerator ChangeTransparencyEnumerator(float target,bool EnableOnStart)
    {
        if (DialogWindowCanvas == null) yield break;
        float _current = DialogWindowCanvas.alpha;
        float _step = target-_current * TransitionStepCooldown;
        if (EnableOnStart && !IsDialogEnabled)
        { 
            DialogWindowCanvas.gameObject.SetActive(true);
        }
        for (float i = 0; i < 1; i += TransitionStepCooldown)
        {
            DialogWindowCanvas.alpha += _step;
            yield return new WaitForSeconds(TransitionStepCooldown);
        }
        yield return new WaitForEndOfFrame();
        if (!EnableOnStart && IsDialogEnabled)
        {
            DialogWindowCanvas.gameObject.SetActive(false);
        }
    }
}
