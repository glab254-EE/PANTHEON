using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CustomInputListenerConnection
{
    internal UnityAction<InputAction.CallbackContext> Callback { get; private set; }
    internal Action<CustomInputListenerConnection> OnDisableAction { get; private set; }
    internal InputActionReference Keybind {  get; private set; }
    internal bool ActivateOnCancel { get; private set; }
    internal bool DeactivateOnFirstInvoke { get; private set; }
    public CustomInputListenerConnection(UnityAction<InputAction.CallbackContext> callback,Action<CustomInputListenerConnection> onDisableAction, InputActionReference keybind, bool activateOnCancel = false, bool deactivateOnFirstInvoke = false)
    {
        OnDisableAction = onDisableAction;
        Keybind = keybind;
        ActivateOnCancel = activateOnCancel;
        DeactivateOnFirstInvoke = deactivateOnFirstInvoke;
        Callback = callback;

        OnActivate();
    }
    public void Disable()
    {
        Keybind.action.performed -= OnKeybindPress;
        Keybind.action.canceled -= OnKeybindPress;
        OnDisableAction(this);
    }
    private void OnKeybindPress(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() || ActivateOnCancel)
        {
            Callback(context);
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
