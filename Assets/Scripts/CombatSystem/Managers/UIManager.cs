using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CombatSystem
{
    public class UIManager : MonoBehaviour
    {
        [Header("Dialogue")]
        [SerializeField] private GameObject buttonsParent;
        [SerializeField] private Button attackButton;
        [SerializeField] private Button chargeButton;
        [SerializeField] private Button defenseButton;

        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("HUD")]
        [SerializeField] private UICharacterHUD playerHUD;
        [SerializeField] private UICharacterHUD enemyHUD;
        [SerializeField] private UIHoverRevealer playerBuffRevealer;

        [Header("Win Lose UI")]
        [SerializeField] private UIWinPanel winPanel;
        public UIWinPanel WinPanel => winPanel;
        [SerializeField] private GameObject loseUI;
        [SerializeField] private Button loseButton;

        [Header("Battle Start UI")]
        [SerializeField] private GameObject battleStartUI;
        [SerializeField] private Button battleStartButton;

        private CombatManager combatManager;
        private CommandManager commandManager;

        public UnityEvent Evt_OnBattleStart = new UnityEvent();

        private void Start()
        {
            commandManager = ServiceLocator.Get<CommandManager>();
            combatManager = ServiceLocator.Get<CombatManager>();

            attackButton.onClick.AddListener(OnPlayerChooseAttack);
            chargeButton.onClick.AddListener(OnPlayerChooseCharge);
            defenseButton.onClick.AddListener(OnPlayerChooseDefense);

            loseButton.onClick.AddListener(OnLosePanelBtnClicked);

            battleStartButton.onClick.AddListener(OnBattleStart);

            winPanel.Evt_BtnOnClickUniversal.AddListener(OnWinPanelBtnClicked);

            HideButtons();
            HideWinUI();
            HideLoseUI();
            ShowBattleStartUI();
        }

        private void OnBattleStart()
        {
            HideBattleStartUI();
            Evt_OnBattleStart.Invoke();
            Invoke("DelayedOnBattleStart", 2f);
        }

        public void DelayedOnBattleStart()
        {
            ShowButtons();
            ShowDialogueMessage("Choose action");
        }

        public void QueueInDialogueTextCommand(string s, float waitTime)
        {
            commandManager.AddCommand(new UIDialogueCommand(s, this, waitTime));
        }

        public void QueueInBtnsVisibilityCommand(float waitTime, bool show)
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

        public void UpdateAttackButtonStatus(int currentCharge, int maxCharge)
        {
            if (currentCharge > 0) attackButton.enabled = true;
            else attackButton.enabled = false;
        }

        public void UpdateDefenseButtonStatus(bool enabled)
        {
            defenseButton.enabled = enabled;
        }

        public void UpdatePlayerHealthBar(int currentVal, int maxVal)
        {
            playerHUD.SetHealthBar((float)currentVal / maxVal);
        }

        public void UpdateEnemyHealthBar(int currentVal, int maxVal)
        {
            enemyHUD.SetHealthBar((float)currentVal / maxVal);
        }

        public void UpdatePlayerChargeBar(int currentVal, int maxVal)
        {
            playerHUD.SetChargeBar((float)currentVal / maxVal);
        }

        public void UpdateEnemyChargeBar(int currentVal, int maxVal)
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

        public void ShowWinUI()
        {
            Invoke("DelayedShowWinUI", 0.2f);
        }

        private void DelayedShowWinUI()
        {
            winPanel.gameObject.SetActive(true);
            HideButtons();
        }

        private void HideWinUI()
        {
            winPanel.gameObject.SetActive(false);
        }

        private void OnWinPanelBtnClicked()
        {
            ShowBattleStartUI();
            combatManager.SetUpBattle();
            HideWinUI();
        }

        private void OnLosePanelBtnClicked()
        {
            ShowBattleStartUI();
            combatManager.SetUpBattle();
            HideLoseUI();
        }

        public void ShowLoseUI()
        {
            Invoke("DelayedShowLoseUI", 0.2f);
        }

        private void DelayedShowLoseUI()
        {
            loseUI.SetActive(true);
            HideButtons();
        }

        private void HideLoseUI()
        {
            loseUI.SetActive(false);
        }

        private void ShowBattleStartUI()
        {
            battleStartUI.SetActive(true);
        }

        private void HideBattleStartUI()
        {
            battleStartUI.SetActive(false);
        }

        public void SetUpPlayerBuffText(string s)
        {
            playerBuffRevealer.SetHoverText(s);
        }
    }
}