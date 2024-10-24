using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterHUD : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _chargeBar;

    public void SetHealthBar(float t)
    {
        _healthBar.value = t;
    }

    public void SetChargeBar(float t)
    {
        _chargeBar.value = t;
    }
}
