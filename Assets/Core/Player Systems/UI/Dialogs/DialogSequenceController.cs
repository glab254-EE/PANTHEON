using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogSequenceController : MonoBehaviour
{
    [Serializable]
    public struct DialogSequenceOption
    {
        public DialogInvoker invoker;
        public string text;
        public bool autoContinue;
        public float autoContinueDelay;
    }
    [SerializeField]
    private PlayerInputListener playerInputListener;
    [SerializeField]
    private InputActionReference proceedKey;
    [SerializeField]
    private UnityEvent completionEvent;
    [field:SerializeField]
    private List<DialogSequenceOption> sequence;
    private bool Activated = false;
    private bool Continue = false;
    public bool SkipTrigger = false;
    public void Trigger()
    {
        if (Activated || sequence == null || proceedKey == null || playerInputListener == null) return;
        StartCoroutine(PrimaryEnumerator());
    }
    public IEnumerator PrimaryEnumerator()
    {
        WaitForSeconds _waitForSeconds0_1 = new(0.1f);
        Activated = true;
        playerInputListener.MockMovementInput = Vector2.zero;
        for (int i = 0; i < sequence.Count; i++)
        {
            DialogSequenceOption option = sequence[i];
            if (option.invoker != null)
            {
                playerInputListener.ConnectEventToKeybind(proceedKey,Skip,false,true);
                StartCoroutine(option.invoker.TriggerAwaitable(option.text));
                do
                {
                    yield return new WaitForEndOfFrame();
                } while (!option.invoker.Completed && SkipTrigger == false);
                if (SkipTrigger == true)
                {
                    option.invoker._dialogWindowController.CancelDisplay();
                    SkipTrigger = false;
                } else
                {
                    playerInputListener.DisableAction(Skip);
                }
                playerInputListener.ConnectEventToKeybind(proceedKey,OnPress,false,true);
                if (option.autoContinue && option.autoContinueDelay > 0)
                {
                    float currentTick = 0;
                    do
                    {
                        yield return _waitForSeconds0_1;
                        currentTick += 0.1f;
                    } while (!Continue && currentTick <= option.autoContinueDelay);
                } else
                {
                    do
                    {
                        yield return new WaitForEndOfFrame();
                    } while (!Continue);
                }
                Continue = false;
                if (i == sequence.Count - 1)
                {
                    StartCoroutine(option.invoker._dialogWindowController.ChangeTransparencyEnumerator(0,false));
                    option.invoker._dialogWindowController.TextField.text = "";
                }
            }
        }
        completionEvent?.Invoke();
        playerInputListener.MockMovementInput = null;
    }
    public void Skip(InputAction.CallbackContext _)
    {
        SkipTrigger = true;
    }
    public void OnPress(InputAction.CallbackContext callbackContext)
    {
        Continue = true;
    }
}
