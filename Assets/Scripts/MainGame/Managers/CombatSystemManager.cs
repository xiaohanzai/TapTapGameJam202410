using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatSystemManager : MonoBehaviour
{
    [SerializeField] private GameObject mainBody;

    [SerializeField] private Button[] winBtns;
    [SerializeField] private Button loseBtn;

    [SerializeField] private CombatSystem.CombatManager combatManager;

    public UnityEvent Evt_OnPlayerWon = new UnityEvent();
    public UnityEvent Evt_OnPlayerLost = new UnityEvent();

    void Start()
    {
        foreach (var btn in winBtns)
            btn.onClick.AddListener(() => { Evt_OnPlayerWon.Invoke(); });
        loseBtn.onClick.AddListener(() => { Evt_OnPlayerLost.Invoke(); });
    }

    public void Activate()
    {
        mainBody.SetActive(true);
    }

    public void Deactivate()
    {
        mainBody.SetActive(false);
    }

    public void StartBossFight()
    {
        Debug.Log("start boss fight...");
    }

    public void StartCombat(int i)
    {
        combatManager.StartCombat(i);
    }
}
