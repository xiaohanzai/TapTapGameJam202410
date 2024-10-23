using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    [SerializeField] private Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth / (float)maxHealth;
    }

    private void OnDestroy()
    {
        //GetComponent<StateController>().Evt_OnBeingAttacked.RemoveListener(LoseHealth);
    }

    public void LoseHealth()
    {
        currentHealth--;
        healthBar.value = currentHealth / (float)maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
