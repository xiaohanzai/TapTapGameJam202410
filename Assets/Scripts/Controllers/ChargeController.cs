using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChargeController : MonoBehaviour
{
    [SerializeField] private int maxCharge;
    private int currentCharge;

    [SerializeField] private Slider chargeBar;

    public UnityEvent<bool> Evt_OnChargeChanged = new UnityEvent<bool>();

    private void Start()
    {
        currentCharge = 0;
        chargeBar.value = currentCharge / (float)maxCharge;
        Evt_OnChargeChanged?.Invoke(currentCharge > 0);
    }

    private void OnDestroy()
    {
        //StateController stateController = GetComponent<StateController>();
        //stateController.Evt_OnAttackActed.RemoveListener(LoseCharge);
        //stateController.Evt_OnBeingCharged.RemoveListener(AddCharge);
    }

    public void AddCharge()
    {
        if (currentCharge < maxCharge) currentCharge++;
        chargeBar.value = currentCharge / (float)maxCharge;
        Evt_OnChargeChanged?.Invoke(currentCharge > 0);
    }

    public void LoseCharge()
    {
        if (currentCharge > 0) currentCharge--;
        chargeBar.value = currentCharge / (float)maxCharge;
        Evt_OnChargeChanged?.Invoke(currentCharge > 0);
    }

    public int GetCurrentCharge()
    {
        return currentCharge;
    }
}
