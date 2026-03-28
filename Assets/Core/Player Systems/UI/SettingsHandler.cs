using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    
    [Serializable]
    public struct SliderToMixerAttribute
    {
        public Slider slider;
        public string attribute;
    } 
    [SerializeField]
    private GameObject pauseFrameObject;
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private List<GameObject> UIFrames;
    [field:SerializeField]
    private List<SliderToMixerAttribute> SliderToAttributeName;
    [field:SerializeField]
    private PlayerInputListener playerInputListener;
    [SerializeField]
    private InputActionReference TogglePauseKeybind;
    private bool IsOpen = false;
    private bool? LastCameraActiveState;
    void Start()
    {
        playerInputListener.ConnectEventToKeybind(TogglePauseKeybind,OnPausePerss);
        foreach (SliderToMixerAttribute pair in SliderToAttributeName)
        {
            pair.slider.onValueChanged.AddListener(value =>
            {
               mixer.SetFloat(pair.attribute,value);
            });
        }
    }
    void OnPausePerss(InputAction.CallbackContext _)
    {
        IsOpen = !IsOpen;
        TogglePauseBool(IsOpen);
    }
    void TogglePauseBool(bool state)
    {
        if (state==false)
        {
            if (LastCameraActiveState != null){
                playerInputListener.MouseLocked = (bool)LastCameraActiveState;
                LastCameraActiveState = null;
            }
            Time.timeScale = 1;
            pauseFrameObject.SetActive(false);
        } else
        {
            ToggleFrame(0);
            LastCameraActiveState??=playerInputListener.MouseLocked;
            playerInputListener.MouseLocked = false;
            Time.timeScale = 0;
            pauseFrameObject.SetActive(true);
        }
    }
    public void ToggleFrame(int index)
    {
        if (index == -2)
        {
            Application.Quit();
        }  
        else if (index == -1)
        {
            TogglePauseBool(false);
        }
        else if (index >= 0 && index < UIFrames.Count)
        {
            for (int i = 0; i < UIFrames.Count; i++)
            {
                if(i == index)
                {
                    UIFrames[i].SetActive(true);
                } else
                {
                    UIFrames[i].SetActive(false);
                }
            }
        }
    }
}