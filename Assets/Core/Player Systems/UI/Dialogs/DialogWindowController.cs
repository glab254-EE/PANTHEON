using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DialogWindowController : MonoBehaviour
{
    [Header("General Settings")]
    [field:SerializeField]
    public TMP_Text TextField {get;private set;}
    [SerializeField]
    private CanvasGroup DialogWindowCanvas;
    [SerializeField]
    private AudioSource DefaultSource;
    [Header("Fade Settings")]
    [SerializeField]
    private float TransitionStepCooldown = 0.05f;
    [Header("Typewritter Settings")]
    [SerializeField]
    private float StartingTime = 0.3f;
    [SerializeField]
    private float TextDisplayTimeAfterShown = 0.3f;
    [SerializeField]
    private float NormalLetterTypingDelay = 0.1f;
    [SerializeField]
    private float PunctuiationTypingDelay = 0.3f;
    private Coroutine currentMainCoroutine;
    private Coroutine currentFadeCoroutine;
    private Coroutine textDisplayCoroutine;
    private string currentText;
    private bool IsDialogEnabled
    {
        get
        {
            if (DialogWindowCanvas != null)
            {
                return DialogWindowCanvas.gameObject.activeSelf;
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
        currentText = text;
        currentMainCoroutine = StartCoroutine(MainTextDisplayCoroutine(text,_clip,PlayPerLetter,overrideSource));
    }
    public IEnumerator DisplayTextFromSequence(string text, AudioClip _clip, bool PlayPerLetter = false, bool disable = false, AudioSource overrideSource = null)
    {
        if (text == null) yield break;
        if (!IsDialogEnabled && currentFadeCoroutine == null)
        {
            currentFadeCoroutine = StartCoroutine(ChangeTransparencyEnumerator(1, true));
            yield return currentFadeCoroutine;
            StopCoroutine(currentFadeCoroutine);
            currentFadeCoroutine = null;
        }
        if (textDisplayCoroutine != null)
        {
            StopCoroutine(textDisplayCoroutine);
            textDisplayCoroutine = null;
        }
        if (currentMainCoroutine != null)
        {
            StopCoroutine(currentMainCoroutine);
            currentMainCoroutine = null;
        }
        currentText = text;
        currentMainCoroutine = StartCoroutine(MainTextDisplayCoroutine(text,_clip,PlayPerLetter,overrideSource,disable));
        yield return currentMainCoroutine;
        if (currentMainCoroutine != null)
        {
            StopCoroutine(currentMainCoroutine);
            currentMainCoroutine = null;
        }
        if (textDisplayCoroutine != null)
        {
            StopCoroutine(textDisplayCoroutine);
            textDisplayCoroutine = null;
        }
    }
    private IEnumerator MainTextDisplayCoroutine(string text, AudioClip _clip = null, bool PlayPerLetter = false, AudioSource overrideSource = null, bool DisableOnEnd = true)
    {
        AudioSource originSource = overrideSource != null ? overrideSource : DefaultSource;
        if (originSource.isPlaying) originSource.Stop();

        if (!IsDialogEnabled && currentFadeCoroutine == null)
        {
            currentFadeCoroutine = StartCoroutine(ChangeTransparencyEnumerator(1, true));
            yield return currentFadeCoroutine;
            StopCoroutine(currentFadeCoroutine);
            currentFadeCoroutine = null;
        }
        yield return new WaitForSeconds(StartingTime);
        if (textDisplayCoroutine != null)
        {
            StopCoroutine(textDisplayCoroutine);
            textDisplayCoroutine = null;
        }
        textDisplayCoroutine = StartCoroutine(TextDisplayCoroutine(text,_clip,PlayPerLetter,overrideSource));
        yield return textDisplayCoroutine;
        yield return new WaitForSeconds(TextDisplayTimeAfterShown);
        if (IsDialogEnabled && DisableOnEnd)
        {
            currentFadeCoroutine = StartCoroutine(ChangeTransparencyEnumerator(0, false));
            yield return currentFadeCoroutine;
            currentFadeCoroutine = null;
        }
        if (currentMainCoroutine != null)
        {
            StopCoroutine(currentMainCoroutine);
            currentMainCoroutine = null;
        }
        if (textDisplayCoroutine != null)
        {
            StopCoroutine(textDisplayCoroutine);
            textDisplayCoroutine = null;
        }
    }
    private IEnumerator TextDisplayCoroutine(string text, AudioClip _clip = null, bool PlayPerLetter = false, AudioSource overrideSource = null)
    {
        AudioSource originSource = overrideSource != null ? overrideSource : DefaultSource;
        if (originSource.isPlaying) originSource.Stop();
        if (!PlayPerLetter && _clip != null)
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
                if (PlayPerLetter && _clip != null) originSource.PlayOneShot(_clip);
                yield return new WaitForSeconds(char.IsPunctuation(c) ? PunctuiationTypingDelay : NormalLetterTypingDelay);
                continue;
            }
        }
        if (!PlayPerLetter && _clip != null)
        {
            originSource.clip = null;
            originSource.Stop();
        }
    }
    public IEnumerator ChangeTransparencyEnumerator(float target,bool EnableOnStart)
    {
        if (DialogWindowCanvas == null) yield break;
        float _current = DialogWindowCanvas.alpha;
        float _step = (target-_current) * TransitionStepCooldown;
        if (EnableOnStart && !IsDialogEnabled)
        { 
            DialogWindowCanvas.gameObject.SetActive(true);
        }
        for (float i = 0; i < 1; i += TransitionStepCooldown)
        {
            DialogWindowCanvas.alpha += _step;
            yield return new WaitForSeconds(TransitionStepCooldown);
        }
        if (!EnableOnStart && IsDialogEnabled)
        {
            DialogWindowCanvas.gameObject.SetActive(false);
        }
    }
    public void CancelDisplay()
    {
        if (currentMainCoroutine != null)
        {
            StopCoroutine(currentMainCoroutine);
            currentMainCoroutine = null;
            if (textDisplayCoroutine != null)
            {
                StopCoroutine(textDisplayCoroutine);
                textDisplayCoroutine = null;
            }
            TextField.text = currentText;
        }
    }
}
