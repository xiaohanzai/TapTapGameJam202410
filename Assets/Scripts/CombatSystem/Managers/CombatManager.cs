using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem;

namespace CombatSystem
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private GameObject battleParent;

        [SerializeField] private PlayerStatsController playerStatsController;
        private int playerCurrentHealth;
        private int playerMaxHealth;
        private int playerCurrentCharge;
        private int playerMaxCharge;
        private int playerCurrentLight;

        private EnemyStatsController enemyStatsController;

        private CommandManager commandManager;
        private UIManager uiManager;
        private EnemyManager enemyManager;

        private CombatContext _combatContext;

        private bool isInitialized;

        void Start()
        {
            _combatContext = new CombatContext();

            commandManager = ServiceLocator.Get<CommandManager>();
            uiManager = ServiceLocator.Get<UIManager>();
            enemyManager = ServiceLocator.Get<EnemyManager>();

            commandManager.Evt_OnCommandsFinished.AddListener(OnCommandsFinished);
            uiManager.Evt_OnBattleStart.AddListener(RevealBuffs);

            playerStatsController.Evt_OnChargeChanged.AddListener(OnPlayerChargeStatsChanged);
            playerStatsController.Evt_OnHealthChanged.AddListener(OnPlayerHealthStatsChanged);
        }

        private void Update()
        {
            if (!isInitialized) SetUpBattle(false);
        }

        public void SetUpBattle(bool isBoss)
        {
            isInitialized = true;
            _combatContext.ClearPlayerActions();
            _combatContext.ClearEnemyActions();
            enemyStatsController = enemyManager.GetAndShowCurrentEnemy();
            if (!isBoss) uiManager.ShowDialogueMessage("A wild enemy has appeared");
            else uiManager.ShowDialogueMessage("This is the final battle with the boss");
            uiManager.SetUpPlayerBuffText("");
            if (enemyStatsController == null)
            {
                Debug.Log("all enemies are defeated");
            }
            else
            {
                enemyStatsController.SetUp();
                playerStatsController.SetUp(playerCurrentHealth, playerCurrentCharge, playerCurrentLight);
                uiManager.WinPanel.SetUpWinPanel(enemyStatsController.GetLightAmount(), enemyStatsController.GetBuffType(), playerStatsController.RevealBuff(false), enemyStatsController.RevealBuff(false));
            }
        }

        public void OnPlayerActionChosen(ActionName playerActionName)
        {
            ActionName enemyActionName = enemyStatsController.ChooseAction(playerActionName, _combatContext);

            _combatContext.AddToPlayerActions(playerActionName);
            _combatContext.AddToEnemyActions(enemyActionName);

            // for simplicity, play animations together and then take damage / charge etc.
            uiManager.QueueInBtnsVisibilityCommand(0, false);

            string s = "<color=\"red\">Player</color> chooses to " + uiManager.ActionName2String(playerActionName) + "\n" + "<color=\"red\">Enemy</color> chooses to " + uiManager.ActionName2String(enemyActionName);
            uiManager.QueueInDialogueTextCommand(s, 1.5f);

            if (playerActionName == ActionName.Defense) playerStatsController.QueueInActionCommand(playerActionName, 0.01f, enemyStatsController);
            if (enemyActionName == ActionName.Defense) enemyStatsController.QueueInActionCommand(enemyActionName, 0.01f, playerStatsController);

            playerStatsController.QueueInAnimationCommand(playerActionName, 0);
            enemyStatsController.QueueInAnimationCommand(enemyActionName,
                Mathf.Max(playerStatsController.AttackAnimationDuration, enemyStatsController.AttackAnimationDuration));

            if (playerActionName != ActionName.Defense) playerStatsController.QueueInActionCommand(playerActionName, 0.01f, enemyStatsController);
            if (enemyActionName != ActionName.Defense) enemyStatsController.QueueInActionCommand(enemyActionName, 0.01f, playerStatsController);

            commandManager.ExecuteCommands();
        }

        private void OnCommandsFinished()
        {
            playerStatsController.RevealCombatResults();
            enemyStatsController.RevealCombatResults();
            if (!enemyStatsController.IsDead())
            {
                //Debug.Log("still alive");
                uiManager.ShowButtons();
                uiManager.ShowDialogueMessage("Choose action");

                playerStatsController.ResetStats();
                enemyStatsController.ResetStats();
            }
            else
            {
                uiManager.ShowDialogueMessage("");
            }
        }

        public void RevealBuffs()
        {
            string s = "<color=\"red\">Player buff:</color> " + playerStatsController.RevealBuff(true) + "\n";
            s += "<color=\"red\">Enemy buff:</color> " + enemyStatsController.RevealBuff(true);
            uiManager.ShowDialogueMessage(s);
            uiManager.SetUpPlayerBuffText(playerStatsController.RevealBuff(false));
        }

        public void StartCombat(int i)
        {
            battleParent.SetActive(true);
            uiManager.HideEnemyUnseenUI();
            enemyManager.SetEnemy(i);
            SetUpBattle(i < 0);
            uiManager.ShowBattleStartUI();
        }

        private void OnPlayerHealthStatsChanged(int currentHealth, int maxHealth)
        {
            playerCurrentHealth = currentHealth;
            playerMaxHealth = maxHealth;
        }

        private void OnPlayerChargeStatsChanged(int currentCharge, int maxCharge)
        {
            playerCurrentCharge = currentCharge;
            playerMaxCharge = maxCharge;
        }

        public (int, int) GetPlayerCurrentStats()
        {
            return (playerCurrentHealth, playerCurrentCharge);
        }

        public void ChangePlayerCurrentStats(int delatHealth, int deltaCharge, int light)
        {
            playerCurrentHealth += delatHealth;
            if (playerCurrentHealth > playerMaxHealth) playerCurrentHealth = playerMaxHealth;
            playerCurrentCharge += deltaCharge;
            if (playerCurrentCharge > playerMaxCharge) playerCurrentCharge = playerMaxCharge;
            playerCurrentLight = light;
        }

        public void ShowEnemyUnseenUI()
        {
            battleParent.SetActive(false);
            uiManager.ShowEnemyUnseenUI();
        }
    }
}