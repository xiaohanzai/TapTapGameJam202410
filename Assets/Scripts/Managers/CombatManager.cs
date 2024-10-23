using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [SerializeField] private PlayerStateController playerStateController;
    [SerializeField] private EnemyStateController enemyStateController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
    }

    private void OnDestroy()
    {
    }

    public void OnPlayerActionChosen(StateName stateName)
    {
        StartCoroutine(Co_OnPlayerActionChosen(stateName));
    }

    private IEnumerator Co_OnPlayerActionChosen(StateName stateName)
    {
        playerStateController.ChangeState(stateName);

        StateName enemyStateName = enemyStateController.DecideNextMove(stateName);
        enemyStateController.ChangeState(enemyStateName);

        UIManager.Instance.UpdateDialogueText(stateName, enemyStateName);
        UIManager.Instance.HideButtons();

        yield return new WaitForSeconds(2f);

        float p = Random.Range(0, 10);
        playerStateController.ExecuteAction(enemyStateController);
        enemyStateController.ExecuteAction(playerStateController);

        yield return new WaitForSeconds(2f);
        UIManager.Instance.ShowButtons();
        UIManager.Instance.ResetDialogueText();

        enemyStateController.BackToIdle();
        playerStateController.BackToIdle();
    }
}
