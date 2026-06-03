using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleUIOnButtonPress : MonoBehaviour
{
    [SerializeField]
    private PlayerInputListener playerInputListener;
    [SerializeField]
    private InputActionReference keybind;
    [SerializeField]
    private GameObject ToToggle;
    void Start()
    {
        playerInputListener.ConnectEventToKeybind(keybind,OnPress);        
    }
    void OnPress(InputAction.CallbackContext _)
    {
        bool nextState = !ToToggle.activeInHierarchy;
        ToToggle.SetActive(nextState);
    }
}
