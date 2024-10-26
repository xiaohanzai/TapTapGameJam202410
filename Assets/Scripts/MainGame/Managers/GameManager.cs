using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FairySystemManager fairySystemManager;
    [SerializeField] private CombatSystemManager combatSystemManager;

    [SerializeField] private float timeLimitToShareLight = 5f;

    [SerializeField] private int maxRounds = 10;
    private int round;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private LightManager lightManager;

    void Start()
    {
        uiManager.Evt_OnShareLightBtnPressed.AddListener(lightManager.SharePlayerLightWithEnvironment);
        uiManager.Evt_OnProceedBtnPressed.AddListener(OnEachRoundProceeds);
        uiManager.Evt_OnAuroraBtnPressed.AddListener(OnAuroraDiscovered);

        combatSystemManager.Evt_OnPlayerWon.AddListener(OnEachRoundStarts);
        combatSystemManager.Evt_OnPlayerLost.AddListener(OnPlayerLost);
        fairySystemManager.Evt_OnEncounterEnded.AddListener(OnFairyEncounterEnded);

        round = -1;
        OnEachRoundStarts();
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

        if (lightManager.CheckIfAuroraVisible(round)) uiManager.ShowAuroraBtn();
        else if (round < maxRounds - 1) uiManager.ShowProceedBtn();
        else uiManager.ShowBossFightBtn();
    }

    private void OnEachRoundProceeds(int ind)
    {
        if (ind == 0)
        {
            fairySystemManager.Activate();
            fairySystemManager.ShowFairy(round);
        }
        else
        {
            combatSystemManager.Activate();
            //combatSystemManager.ShowEnemy(round);
        }
    }

    private void OnAuroraDiscovered()
    {
        combatSystemManager.StartBossFight();
    }

    private void OnFairyEncounterEnded()
    {
        fairySystemManager.Deactivate();
    }

    private void OnPlayerLost()
    {
        round = -1;
        Debug.Log("game needs to restart");
    }
}
