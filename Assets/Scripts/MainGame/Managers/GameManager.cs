using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FairySystemManager fairySystemManager;
    [SerializeField] private CombatSystemManager combatSystemManager;
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LightManager lightManager;

    [SerializeField] private float timeLimitToShareLight = 5f;

    [SerializeField] private int maxRounds = 10;
    private int round;

    [SerializeField] private int playerInitLight;
    [SerializeField] private int playerMaxLight;
    [SerializeField] private int environmentMaxLight;

    void Start()
    {
        uiManager.Evt_OnShareLightBtnPressed.AddListener(lightManager.SharePlayerLightWithEnvironment);
        uiManager.Evt_OnProceedBtnPressed.AddListener(OnEachRoundProceeds);
        uiManager.Evt_OnAuroraBtnPressed.AddListener(OnAuroraDiscovered);
        uiManager.Evt_OnFakeBossFightClicked.AddListener(StartFakeBossFight);

        combatSystemManager.Evt_OnPlayerWon.AddListener(OnEachRoundStarts);
        combatSystemManager.Evt_OnPlayerLost.AddListener(OnPlayerLost);

        fairySystemManager.Evt_OnEncounterEnded.AddListener(OnFairyEncounterEnded);

        RestartGame();
    }

    private void OnEachRoundStarts()
    {
        round++;
        fairySystemManager.Deactivate();
        combatSystemManager.Deactivate();
        StartCoroutine(Co_OnEachRoundStarts());
    }

    IEnumerator Co_OnEachRoundStarts()
    {
        uiManager.ShowShareLightBtn();

        float t = timeLimitToShareLight;
        while (t > 0)
        {
            uiManager.UpdateCountDownText(t);
            t -= Time.deltaTime;
            yield return null;
        }

        uiManager.UpdateCountDownText(-1);
        uiManager.HideShareLightBtn();

        if (lightManager.CheckIfAuroraVisible(round, maxRounds)) uiManager.ShowAuroraBtn();
        else if (round < maxRounds - 1) uiManager.ShowProceedBtn();
        else uiManager.ShowFakeBossFightBtn();
    }

    private void OnEachRoundProceeds(int ind)
    {
        if (ind == 0)
        {
            fairySystemManager.Activate();
            if (lightManager.IsFairySeeable()) fairySystemManager.ShowFairy(round);
            else fairySystemManager.ShowFairyUnseenUI();
        }
        else
        {
            combatSystemManager.Activate();
            if (lightManager.IsEnemySeeable()) combatSystemManager.StartCombat(round);
            else combatSystemManager.ShowEnemyUnseenUI();
        }
    }

    private void OnAuroraDiscovered()
    {
        combatSystemManager.Activate();
        combatSystemManager.StartBossFight();
    }

    private void OnFairyEncounterEnded()
    {
        if (fairySystemManager.IsFairyMet)
        {
            int minHP, maxHP;
            (minHP, maxHP) = fairySystemManager.GetCurrentFairyHPs();
            int hp = playerStatsManager.GenerateRandomHPBack(minHP, maxHP);
            int light = lightManager.GetPlayerLight();
            if (hp > 0)
            {
                combatSystemManager.ChangePlayerStats(hp, 0, light);
                uiManager.ShowFairyEncounterText("Fairy gave you " + hp.ToString() + " HP back");
            }
        }
        fairySystemManager.Deactivate();
    }

    private void OnPlayerLost()
    {
        RestartGame();
    }

    private void RestartGame()
    {
        lightManager.SetUpParams(playerInitLight, playerMaxLight, environmentMaxLight);
        round = -1;
        OnEachRoundStarts();
    }

    private void StartFakeBossFight()
    {
        combatSystemManager.StartFakeBossFight();
    }
}
