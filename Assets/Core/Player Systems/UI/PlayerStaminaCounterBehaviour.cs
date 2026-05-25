using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerStaminaCounterBehaviour : MonoBehaviour
{
    [SerializeField]
    private StaminaBehaviour Stamina;
    [SerializeField]
    private double MaxStamina = 100;
    private Image image;
    void Start()
    {
        image = GetComponent<Image>();
        Stamina.OnStaminaChange += OnStaminaChange;
    }
    void OnDestroy()
    {
        Stamina.OnStaminaChange -= OnStaminaChange;        
    }
    private void OnStaminaChange(double stamina)
    {
        image.fillAmount = (float)(stamina/MaxStamina);
    }
}
