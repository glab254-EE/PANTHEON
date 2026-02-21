using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputListener : MonoBehaviour
{
    [field:Header("Set-Up")]
    [field:SerializeField]
    private InputActionReference inputActionForCameraLock;
    [field: SerializeField]
    private InputActionReference inputActionForEnpowering;
    [field:SerializeField]
    internal Transform CameraTransform {get; private set;}
    internal Vector2 AxisOutput {get;private set;}= Vector2.zero;
    internal Vector3 MovementVector3 {get;private set;}= Vector3.zero;
    internal bool MouseLocked = false;
    internal bool EnpowerButtonHeld = false;
    private Dictionary<InputActionReference, List<CustomInputListenerConnection>> connections = new();
    private InputSystem_Actions inputActions;
    void Start()
    {
        inputActions??=new(); 

        ConnectEventToKeybind(inputActionForCameraLock,OnMouseLock);
        ConnectEventToKeybind(inputActionForEnpowering, OnEnpower,true);
        inputActions.Player.Enable(); 
    }
    void OnDestroy()
    {
        inputActions.Player.Disable();         
    }
    void Update()
    {
        AxisOutput = inputActions.Player.Move.ReadValue<Vector2>();

        if (CameraTransform == null)
        {
            CameraTransform = Camera.main.transform;
        }

        Vector3 _movementVector = CameraTransform.forward*AxisOutput.y+CameraTransform.right*AxisOutput.x;

        _movementVector.y = 0;

        MovementVector3 = _movementVector.normalized;
    }
    public void ConnectEventToKeybind(InputActionReference keybind, UnityAction<InputAction.CallbackContext> action, bool activateOnCancel = false, bool once = false)
    {
        if (!connections.ContainsKey(keybind))
        {
            connections.Add(keybind, new());
        }
        connections[keybind].Add(new(action, DisableAction, keybind, activateOnCancel, once));
    }
    public void DisableAction(CustomInputListenerConnection connection)
    {
        if (connections.ContainsKey(connection.Keybind) && connections[connection.Keybind].Contains(connection))
        {
            connections[connection.Keybind].Remove(connection);
            if (connections[connection.Keybind].Count == 0)
            {
                connections.Remove(connection.Keybind);
                connection.Keybind.action.Disable();
            }
        }
    }
    public void DisableAction(UnityAction<InputAction.CallbackContext> action)
    {
        foreach (List<CustomInputListenerConnection> list in connections.Values)
        {
            foreach(CustomInputListenerConnection connection in list)
            {
                if (connection.Callback == action)
                {
                    connection.Disable();
                    return;
                }
            }
        }
    }
    private void OnMouseLock(InputAction.CallbackContext callBack)
    {
        if (!callBack.ReadValueAsButton()) return;
        MouseLocked = !MouseLocked;
    }
    private void OnEnpower(InputAction.CallbackContext callBack)
    {
        EnpowerButtonHeld = callBack.ReadValueAsButton();
        Debug.Log(EnpowerButtonHeld);
    }
}
