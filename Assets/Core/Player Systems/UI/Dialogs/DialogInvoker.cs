using UnityEngine;

public class DialogInvoker : MonoBehaviour
{
    [SerializeField]
    private DialogWindowController _dialogWindowController;
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private AudioSource _source;
    [SerializeField]
    private bool _playClipPerLetter;
    [SerializeField]
    private string _targetDialogText;
    public void Trigger()
    {
        if (_dialogWindowController == null || _targetDialogText == null || _targetDialogText == "") return;
        _dialogWindowController.DisplayText(_targetDialogText, _clip, _playClipPerLetter,_source);
    }
    public void Trigger(string _text)
    {
        if (_dialogWindowController == null || _text == null || _text == "") return;
        _dialogWindowController.DisplayText(_text, _clip, _playClipPerLetter, _source);
    }
}
