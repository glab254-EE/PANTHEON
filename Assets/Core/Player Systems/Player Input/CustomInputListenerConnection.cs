using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CustomInputListenerConnection
{
    internal UnityEvent<InputAction.CallbackContext> Callback { get; private set; } = new();
    internal Action<CustomInputListenerConnection> OnDisableAction { get; private set; }
    internal InputActionReference Keybind {  get; private set; }
    internal bool ActivateOnCancel { get; private set; }
    internal bool DeactivateOnFirstInvoke { get; private set; }
    internal float MaxHoldDuration { get; private set; }
    public CustomInputListenerConnection(UnityAction<InputAction.CallbackContext> callback,Action<CustomInputListenerConnection> onDisableAction, InputActionReference keybind, bool activateOnCancel = false, bool deactivateOnFirstInvoke = false, float maxDuration = float.MaxValue)
    {
        OnDisableAction = onDisableAction;
        Keybind = keybind;
        ActivateOnCancel = activateOnCancel;
        DeactivateOnFirstInvoke = deactivateOnFirstInvoke;
        Callback.AddListener(callback);
        MaxHoldDuration = maxDuration;

        OnActivate();
    }
    public void Disable()
    {
        Callback.RemoveAllListeners();
        Keybind.action.performed -= OnKeybindPress;
        Keybind.action.canceled -= OnKeybindPress;
        OnDisableAction(this);
    }
    public void Disable(UnityAction<InputAction.CallbackContext> action)
    {
        Callback.RemoveListener(action);
        Keybind.action.performed -= OnKeybindPress;
        Keybind.action.canceled -= OnKeybindPress;
        OnDisableAction(this);
    }
    private void OnKeybindPress(InputAction.CallbackContext context)
    {
        if (context.duration >= MaxHoldDuration) return;
        if (context.ReadValueAsButton() || ActivateOnCancel)
        {
            if (Callback == null) return;
            Callback.Invoke(context);
            if (DeactivateOnFirstInvoke)
            {
                Disable();
            }
        }
    }
    private void OnActivate()
    {
        Keybind.action.performed += OnKeybindPress;
        Keybind.action.canceled += OnKeybindPress;

        if (!Keybind.action.enabled)
        {
            Keybind.action.Enable();
        }
    }
}
