using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject buttonsParent;
        [SerializeField] private Button attackButton;
        [SerializeField] private Button chargeButton;
        [SerializeField] private Button defenseButton;

        [SerializeField] private TextMeshProUGUI dialogueText;

        [SerializeField] private UICharacterHUD playerHUD;
        [SerializeField] private UICharacterHUD enemyHUD;

        [SerializeField] private CombatManager combatManager;

        [SerializeField] private PlayerStatsController playerStatsController;
        [SerializeField] private EnemyStatsController enemyStatsController;

        void Awake()
        {
            attackButton.onClick.AddListener(OnPlayerChooseAttack);
            chargeButton.onClick.AddListener(OnPlayerChooseCharge);
            defenseButton.onClick.AddListener(OnPlayerChooseDefense);

            playerStatsController.Evt_OnChargeChanged.AddListener(UpdateAttackButtonStatus);

            playerStatsController.Evt_OnHealthChanged.AddListener(UpdatePlayerHealthBar);
            playerStatsController.Evt_OnChargeChanged.AddListener(UpdatePlayerChargeBar);
            enemyStatsController.Evt_OnHealthChanged.AddListener(UpdateEnemyHealthBar);
            enemyStatsController.Evt_OnChargeChanged.AddListener(UpdateEnemyChargeBar);
        }

        private void OnDestroy()
        {
            attackButton.onClick.RemoveListener(OnPlayerChooseAttack);
            chargeButton.onClick.RemoveListener(OnPlayerChooseCharge);
            defenseButton.onClick.RemoveListener(OnPlayerChooseDefense);

            playerStatsController.Evt_OnChargeChanged.RemoveListener(UpdateAttackButtonStatus);

            playerStatsController.Evt_OnHealthChanged.RemoveListener(UpdatePlayerHealthBar);
            playerStatsController.Evt_OnChargeChanged.RemoveListener(UpdatePlayerChargeBar);
            enemyStatsController.Evt_OnHealthChanged.RemoveListener(UpdateEnemyHealthBar);
            enemyStatsController.Evt_OnChargeChanged.RemoveListener(UpdateEnemyChargeBar);
        }

        void Update()
        {

        }

        public void QueueInDialogueTextCommand(string s, float waitTime, CommandManager commandManager)
        {
            commandManager.AddCommand(new UIDialogueCommand(s, this, waitTime));
        }

        public void QueueInBtnsVisibilityCommand(float waitTime, bool show, CommandManager commandManager)
        {
            if (show)
            {
                commandManager.AddCommand(new ShowCommand(buttonsParent, waitTime));
            }
            else
            {
                commandManager.AddCommand(new HideCommand(buttonsParent, waitTime));
            }
        }

        private void UpdateAttackButtonStatus(int currentCharge, int maxCharge)
        {
            if (currentCharge > 0) attackButton.enabled = true;
            else attackButton.enabled = false;
        }

        private void UpdatePlayerHealthBar(int currentVal, int maxVal)
        {
            playerHUD.SetHealthBar((float)currentVal / maxVal);
        }

        private void UpdateEnemyHealthBar(int currentVal, int maxVal)
        {
            enemyHUD.SetHealthBar((float)currentVal / maxVal);
        }

        private void UpdatePlayerChargeBar(int currentVal, int maxVal)
        {
            playerHUD.SetChargeBar((float)currentVal / maxVal);
        }

        private void UpdateEnemyChargeBar(int currentVal, int maxVal)
        {
            enemyHUD.SetChargeBar((float)currentVal / maxVal);
        }

        private void OnPlayerChooseAttack()
        {
            combatManager.OnPlayerActionChosen(ActionName.Attack);
        }

        private void OnPlayerChooseDefense()
        {
            combatManager.OnPlayerActionChosen(ActionName.Defense);
        }

        private void OnPlayerChooseCharge()
        {
            combatManager.OnPlayerActionChosen(ActionName.Charge);
        }

        public void ShowButtons()
        {
            buttonsParent.SetActive(true);
        }

        public void HideButtons()
        {
            buttonsParent.SetActive(false);
        }

        public string ActionName2String(ActionName actionName)
        {
            switch (actionName)
            {
                case ActionName.Attack:
                    return "attack";
                case ActionName.Defense:
                    return "defend";
                case ActionName.Charge:
                    return "charge";
                default:
                    return "";
            }
        }

        public void ResetDialogueText()
        {
            dialogueText.text = "Choose an action";
        }

        public void ShowDialogueMessage(string s)
        {
            dialogueText.text = s;
        }
    }
}