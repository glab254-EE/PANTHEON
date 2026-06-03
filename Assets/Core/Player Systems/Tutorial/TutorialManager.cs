using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [Serializable]
    public struct TutorialSection
    {
        public string Message;
        public InputActionReference Input;
        public UnityEvent CompletionEvent;
        public float AutoCancelTime;
        public float DelayAfterSection;
        public float MaxHoldDuration;
    }
    [SerializeField] private List<TutorialSection> TutorialScreens;
    [SerializeField] private DialogInvoker invoker;
    [SerializeField] private PlayerInputListener listener;
    #if UNITY_EDITOR
    [SerializeField] private bool SkipTutorial;
    #endif
    private int currentIndex = 0;
    private bool skipping = false;
    private float CancelTime = 0;
    void Start()
    {
    #if UNITY_EDITOR
        if (SkipTutorial) return;
    #endif
        Debug.Log("Tutorial start!");
        StartCoroutine(DisplayEnumerator());
    }
    void Update()
    {
        if (CancelTime > 0)
        {
            CancelTime -= Time.deltaTime;
        }
    }
    public void Skip()
    {
        skipping = true;
    }
    public void Skip(InputAction.CallbackContext _)
    {
        Debug.Log("NEEXTXXT!");
        skipping = true;
    }
    private IEnumerator DisplayEnumerator()
    {
        invoker._dialogWindowController.TextField.text = "";
        TutorialSection _currentSection = TutorialScreens[currentIndex];
        if (!invoker._dialogWindowController.IsDialogEnabled)
        {
            yield return StartCoroutine(invoker._dialogWindowController.ChangeTransparencyEnumerator(1,true));
        }
        if (_currentSection.Message != null && _currentSection.Message != default)
        {
            yield return StartCoroutine(invoker.TriggerAwaitable(_currentSection.Message));
        }
        if (_currentSection.Input != null)
        {
            listener.ConnectEventToKeybind(_currentSection.Input,Skip,false,true,_currentSection.MaxHoldDuration);
        }
        if (_currentSection.AutoCancelTime > 0)
        {
            CancelTime = _currentSection.AutoCancelTime;
        }
        yield return new WaitWhile(() =>
        {
            return !skipping && (_currentSection.AutoCancelTime <= 0 || CancelTime <= 0);
        });
        CancelTime = 0;
        skipping = false;
        yield return new WaitForSecondsRealtime(Mathf.Abs(_currentSection.DelayAfterSection));
        _currentSection.CompletionEvent?.Invoke();
        if (_currentSection.AutoCancelTime > 0 && _currentSection.Input != null && CancelTime <= 0)
        {
            listener.DisableAction(Skip);
        }
        if (currentIndex+1 < TutorialScreens.Count)
        {
            TutorialSection next = TutorialScreens[currentIndex+1];
            if (next.Input == null && next.AutoCancelTime <= 0)
            {
                yield return StartCoroutine(invoker._dialogWindowController.ChangeTransparencyEnumerator(0,false));  
                yield return new WaitUntil(()=>skipping);            
            }
            currentIndex++;
            StartCoroutine(DisplayEnumerator());
        } else
        {
            yield return StartCoroutine(invoker._dialogWindowController.ChangeTransparencyEnumerator(0,false));  
            Destroy(gameObject);
        }
    }
}
