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
        private float playerCurrentHealth;
        private int playerMaxHealth;
        private int playerCurrentCharge;
        private int playerMaxCharge;

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
            if (!isInitialized) SetUpBattle(false, 1, true);
        }

        public void SetUpBattle(bool isBoss, float fac, bool isChargeMemLost)
        {
            isInitialized = true;
            _combatContext.ClearPlayerActions();
            _combatContext.ClearEnemyActions();
            enemyStatsController = enemyManager.GetAndShowCurrentEnemy();
            if (!isBoss) uiManager.ShowDialogueMessage("敌人出现了");
            else uiManager.ShowDialogueMessage("这是最终的boss战");
            uiManager.SetUpPlayerBuffText("");
            if (enemyStatsController == null)
            {
                Debug.Log("all enemies are defeated");
            }
            else
            {
                enemyStatsController.SetUp(fac);
                playerStatsController.SetUp(playerCurrentHealth, isChargeMemLost? 0 : playerCurrentCharge, fac);
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

            string s = "<color=\"red\">玩家</color> 选择 " + uiManager.ActionName2String(playerActionName) + "\n" + "<color=\"red\">敌人</color> 选择 " + uiManager.ActionName2String(enemyActionName);
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
                uiManager.ShowDialogueMessage("选择下一步行动");

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
            string s = "<color=\"red\">玩家技能:</color> " + playerStatsController.RevealBuff(true) + "\n";
            s += "<color=\"red\">敌人技能:</color> " + enemyStatsController.RevealBuff(true);
            uiManager.ShowDialogueMessage(s);
            uiManager.SetUpPlayerBuffText(playerStatsController.RevealBuff(false));
        }

        public void StartCombat(int i, float fac, bool isChargeMemLost)
        {
            battleParent.SetActive(true);
            uiManager.HideEnemyUnseenUI();
            enemyManager.SetEnemy(i);
            SetUpBattle(i < 0, fac, isChargeMemLost);
            uiManager.ShowBattleStartUI();
        }

        private void OnPlayerHealthStatsChanged(float currentHealth, int maxHealth)
        {
            playerCurrentHealth = currentHealth;
            playerMaxHealth = maxHealth;
        }

        private void OnPlayerChargeStatsChanged(int currentCharge, int maxCharge)
        {
            playerCurrentCharge = currentCharge;
            playerMaxCharge = maxCharge;
        }

        public (float, int) GetPlayerCurrentStats()
        {
            return (playerCurrentHealth, playerCurrentCharge);
        }

        public void ChangePlayerCurrentStats(int delatHealth, int deltaCharge)
        {
            playerCurrentHealth += delatHealth;
            if (playerCurrentHealth > playerMaxHealth) playerCurrentHealth = playerMaxHealth;
            playerCurrentCharge += deltaCharge;
            if (playerCurrentCharge > playerMaxCharge) playerCurrentCharge = playerMaxCharge;
        }

        public void ShowEnemyUnseenUI()
        {
            battleParent.SetActive(false);
            uiManager.ShowEnemyUnseenUI();
        }

        public string GetNextEnemyDescription()
        {
            return enemyStatsController.GetDescription();
        }
    }
}