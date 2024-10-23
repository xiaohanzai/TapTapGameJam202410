using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem;

namespace CombatSystem
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatsController playerStatsController;
        [SerializeField] private EnemyStatsController enemyStatsController;

        [SerializeField] private CommandManager commandManager;
        [SerializeField] private UIManager uiManager;

        void Start()
        {
        }

        private void OnDestroy()
        {
        }

        public void OnPlayerActionChosen(ActionName playerActionName)
        {
            ActionName enemyActionName = enemyStatsController.ChooseAction(playerActionName);

            // for simplicity, play animations together and then take damage / charge etc.
            uiManager.QueueInBtnsVisibilityCommand(0, false, commandManager);

            string s = "Player chooses to " + uiManager.ActionName2String(playerActionName) + "\n" + "Enemy chooses to " + uiManager.ActionName2String(enemyActionName);
            uiManager.QueueInDialogueTextCommand(s, 1.5f, commandManager);

            playerStatsController.QueueInAnimationCommand(playerActionName, 0, commandManager);
            enemyStatsController.QueueInAnimationCommand(enemyActionName,
                Mathf.Max(playerStatsController.AttackAnimationDuration, enemyStatsController.AttackAnimationDuration), commandManager);

            playerStatsController.QueueInActionCommand(playerActionName, 0, enemyStatsController, commandManager);
            enemyStatsController.QueueInActionCommand(enemyActionName, 0, playerStatsController, commandManager);

            uiManager.QueueInBtnsVisibilityCommand(0, true, commandManager);
            uiManager.QueueInDialogueTextCommand("Choose action", 0, commandManager);

            playerStatsController.QueueInResetStatsCommand(commandManager);
            enemyStatsController.QueueInResetStatsCommand(commandManager);

            commandManager.ExecuteCommands();
        }
    }
}