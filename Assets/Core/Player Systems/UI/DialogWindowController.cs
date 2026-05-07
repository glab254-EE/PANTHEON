using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DialogWindowController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField]
    private TMP_Text TextField;
    [SerializeField]
    private CanvasGroup DialogWindowCanvas;
    [SerializeField]
    private AudioSource DefaultSource;
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
    private Coroutine currentMainCoroutine;
    private Coroutine currentFadeCoroutine;
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
    public void DisplayText(string text, AudioClip _clip, bool PlayPerLetter = false, AudioSource overrideSource = null)
    {
        if (text == null || _clip == null) return;
        if (currentMainCoroutine != null)
        {
            StopCoroutine(currentMainCoroutine);
            currentMainCoroutine = null;
        }
        currentMainCoroutine = StartCoroutine(TextDisplayCoroutine(text,_clip,PlayPerLetter,overrideSource));
    }
    public IEnumerator TextDisplayCoroutine(string text, AudioClip _clip, bool PlayPerLetter = false, AudioSource overrideSource = null)
    {
        AudioSource originSource = overrideSource != null ? overrideSource : DefaultSource;
        if (originSource.isPlaying) originSource.Stop();

        if (!IsDialogEnabled && currentFadeCoroutine == null)
        {
            currentFadeCoroutine = StartCoroutine(ChangeTransparencyEnumerator(0, true));
            yield return currentFadeCoroutine;
            currentFadeCoroutine = null;
        }
        if (!PlayPerLetter)
        {
            originSource.clip = _clip;
            originSource.Play();
        }
        string current = "";
        foreach (char c in text)
        {
            current += c;
            TextField.text = current;
            if (!char.IsWhiteSpace(c))
            {
                if (PlayPerLetter) originSource.PlayOneShot(_clip);
                yield return new WaitForSeconds(char.IsPunctuation(c) ? PunctuiationTypingDelay : NormalLetterTypingDelay);
            }
        }
        if (!PlayPerLetter)
        {
            originSource.clip = null;
            originSource.Stop();
        }
        if (IsDialogEnabled && currentFadeCoroutine == null)
        {
            currentFadeCoroutine = StartCoroutine(ChangeTransparencyEnumerator(1, false));
            yield return currentFadeCoroutine;
            currentFadeCoroutine = null;
        }
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
