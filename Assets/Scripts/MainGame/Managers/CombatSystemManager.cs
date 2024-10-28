using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatSystemManager : MonoBehaviour
{
    [SerializeField] private GameObject mainBody;

    [SerializeField] private Button winBossBtn;
    [SerializeField] private Button[] winBtns;
    [SerializeField] private Button loseBtn;
    [SerializeField] private Button proceedBtn;

    [SerializeField] private CombatSystem.CombatManager combatManager;

    public UnityEvent Evt_OnPlayerWonBoss = new UnityEvent();
    public UnityEvent Evt_OnPlayerWon = new UnityEvent();
    public UnityEvent Evt_OnPlayerLost = new UnityEvent();

    void Start()
    {
        winBossBtn.onClick.AddListener(() => { Evt_OnPlayerWonBoss.Invoke(); });
        foreach (var btn in winBtns)
            btn.onClick.AddListener(() => { Evt_OnPlayerWon.Invoke(); });
        loseBtn.onClick.AddListener(() => { Evt_OnPlayerLost.Invoke(); });
        proceedBtn.onClick.AddListener(() => { Evt_OnPlayerWon.Invoke(); });
    }

    public void Activate()
    {
        mainBody.SetActive(true);
    }

    public void Deactivate()
    {
        mainBody.SetActive(false);
    }

    public void StartBossFight(float fac, bool isChargeMemLost)
    {
        combatManager.StartCombat(-1, fac, isChargeMemLost);
    }

    public void StartFakeBossFight(float fac, bool isChargeMemLost)
    {
        combatManager.StartCombat(-2, fac, isChargeMemLost);
    }

    public void StartCombat(int i, float fac, bool isChargeMemLost)
    {
        combatManager.StartCombat(i, fac, isChargeMemLost);
    }

    public void ChangePlayerStats(int deltaHealth, int deltaCharge)
    {
        combatManager.ChangePlayerCurrentStats(deltaHealth, deltaCharge);
    }

    public (float, int) GetPlayerStats()
    {
        return combatManager.GetPlayerCurrentStats();
    }

    public void ShowEnemyUnseenUI()
    {
        combatManager.ShowEnemyUnseenUI();
    }

    public string GetNextEnemyDescription()
    {
        return combatManager.GetNextEnemyDescription();
    }
}
