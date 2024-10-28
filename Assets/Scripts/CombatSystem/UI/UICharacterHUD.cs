using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICharacterHUD : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _chargeBar;

    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _chargeText;

    public void SetHealthBar(float currentVal, int maxVal)
    {
        _healthBar.value = currentVal / maxVal;
        _healthText.text = currentVal.ToString("0.0") + " / " + maxVal.ToString();
    }

    public void SetChargeBar(int currentVal, int maxVal)
    {
        _chargeBar.value = (float)currentVal / maxVal;
        _chargeText.text = currentVal.ToString() + " / " + maxVal.ToString();
    }
}
