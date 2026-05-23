using System.Collections;
using UnityEngine;

public class DialogInvoker : MonoBehaviour
{
    [field:SerializeField]
    public DialogWindowController _dialogWindowController {get;private set;}
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private AudioSource _source;
    [SerializeField]
    private bool _playClipPerLetter;
    [SerializeField]
    private string _targetDialogText;
    public bool Completed {get;private set;}
    public void Trigger()
    {
        Completed = false;
        if (_dialogWindowController == null || _targetDialogText == null || _targetDialogText == "") return;
        _dialogWindowController.DisplayText(_targetDialogText, _clip, _playClipPerLetter,_source);
        Completed = true;
    }
    public void Trigger(string _text)
    {
        Completed = false;
        if (_dialogWindowController == null || _text == null || _text == "") return;
        _dialogWindowController.DisplayText(_text, _clip, _playClipPerLetter, _source);
        Completed = true;
    }
    public IEnumerator TriggerAwaitable(string _text, bool DisableAfterwards = false)
    {
        Completed = false;
        if (_dialogWindowController == null || _text == null || _text == "") yield break;
        yield return StartCoroutine(_dialogWindowController.DisplayTextFromSequence(_text,_clip,_playClipPerLetter,DisableAfterwards,_source));
        Completed = true;
    }
}
