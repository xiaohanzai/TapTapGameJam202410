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
    [SerializeField] private BGMManager bgmManager;

    [SerializeField] private float timeLimitToShareLight = 5f;

    [SerializeField] private int maxRounds = 10;
    private int round;

    private bool isChargeMemLost;

    [SerializeField] private int playerInitLight = 10;
    [SerializeField] private int environmentMaxLight = 20;
    [SerializeField] private float lightThreshold1 = 10;
    [SerializeField] private float lightThreshold2 = 20;

    private int nDeaths;
    private float fac;

    void Start()
    {
        uiManager.Evt_OnShareLightBtnPressed.AddListener(lightManager.SharePlayerLightWithEnvironment);
        uiManager.Evt_OnProceedBtnPressed.AddListener(OnEachRoundProceeds);
        uiManager.Evt_OnAuroraBtnPressed.AddListener(OnAuroraDiscovered);
        uiManager.Evt_OnFakeBossFightClicked.AddListener(StartFakeBossFight);
        uiManager.Evt_OnStartGameBtnPressed.AddListener(RestartGame);

        combatSystemManager.Evt_OnPlayerWonBoss.AddListener(() => { uiManager.ShowMainMenuUI(); });
        combatSystemManager.Evt_OnPlayerWon.AddListener(OnEachRoundStarts);
        combatSystemManager.Evt_OnPlayerLost.AddListener(OnPlayerLost);

        fairySystemManager.Evt_OnEncounterEnded.AddListener(OnFairyEncounterEnded);

        //RestartGame();
    }

    private void OnEachRoundStarts()
    {
        round++;
        fairySystemManager.Deactivate();
        combatSystemManager.Deactivate();
        bgmManager.PlayMainGameBGM();
        StartCoroutine(Co_OnEachRoundStarts());
    }

    IEnumerator Co_OnEachRoundStarts()
    {
        yield return null;

        if (round > 0) lightManager.SetPayerCurrentLight(combatSystemManager.GetPlayerStats().Item1);
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
            bgmManager.PlayFairyBGM();
            fairySystemManager.Activate();
            if (lightManager.IsFairySeeable(round)) fairySystemManager.ShowFairy(round);
            else fairySystemManager.ShowFairyUnseenUI();
        }
        else
        {
            bgmManager.PlayBattleBGM();
            combatSystemManager.Activate();
            if (lightManager.IsEnemySeeable(round)) combatSystemManager.StartCombat(round, fac, isChargeMemLost);
            else combatSystemManager.ShowEnemyUnseenUI();
        }
    }

    private void OnAuroraDiscovered()
    {
        combatSystemManager.Activate();
        combatSystemManager.StartBossFight(fac, isChargeMemLost);
    }

    private void OnFairyEncounterEnded()
    {
        if (fairySystemManager.IsFairyMet)
        {
            int minHP, maxHP;
            (minHP, maxHP) = fairySystemManager.GetCurrentFairyHPs();
            int hp = playerStatsManager.GenerateRandomHPBack(minHP, maxHP);
            //if (hp > 0)
            //{
            combatSystemManager.ChangePlayerStats(hp - lightManager.GetDeltaLight(), 0);
            float playerLight = combatSystemManager.GetPlayerStats().Item1;
            lightManager.SetPayerCurrentLight(playerLight);
            float t = 2f;
            //}
            if (lightManager.GetEnvironmentLight() > lightThreshold1)
            {
                t = 6f;
                if (lightManager.GetEnvironmentLight() < lightThreshold2) isChargeMemLost = true;
                else isChargeMemLost = false;
            }
            uiManager.ShowFairyEncounterText(hp, playerLight, t > 3? combatSystemManager : null, isChargeMemLost, t);
        }
        fairySystemManager.Deactivate();
    }

    private void OnPlayerLost()
    {
        nDeaths++;
        uiManager.ShowMainMenuUI();
    }

    private void RestartGame()
    {
        fac = Mathf.Max(1f - 0.1f * nDeaths, 0.6f);
        lightManager.SetUpParams(playerInitLight, environmentMaxLight);
        round = -1;
        isChargeMemLost = true;
        OnEachRoundStarts();
    }

    private void StartFakeBossFight()
    {
        combatSystemManager.StartFakeBossFight(fac, isChargeMemLost);
    }
}
