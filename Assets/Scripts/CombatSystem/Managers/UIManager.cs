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
        [SerializeField] private GameObject bossWinPanel;
        //[SerializeField] private Button bossWinButton;
        [SerializeField] private GameObject fakeBossWinPanel;
        [SerializeField] private Button fakeBossWinButton;
        [SerializeField] private UIWinPanel winPanel;
        public UIWinPanel WinPanel => winPanel;
        [SerializeField] private GameObject loseUI;
        [SerializeField] private Button loseButton;

        [Header("Battle Start UI")]
        [SerializeField] private GameObject battleStartUI;
        [SerializeField] private Button battleStartButton;

        [Header("Enemy Unseen UI")]
        [SerializeField] private GameObject enemyUnseenUI;

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
            fakeBossWinButton.onClick.AddListener(OnLosePanelBtnClicked);

            battleStartButton.onClick.AddListener(OnBattleStart);

            winPanel.Evt_BtnOnClickUniversal.AddListener(OnWinPanelBtnClicked);

            HideButtons();
            HideWinUI();
            HideLoseUI();
            HideBattleStartUI();
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
            ShowDialogueMessage("选择下一步行动");
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

        public void UpdatePlayerHealthBar(float currentVal, int maxVal)
        {
            playerHUD.SetHealthBar(currentVal, maxVal);
        }

        public void UpdateEnemyHealthBar(float currentVal, int maxVal)
        {
            enemyHUD.SetHealthBar(currentVal, maxVal);
        }

        public void UpdatePlayerChargeBar(int currentVal, int maxVal)
        {
            playerHUD.SetChargeBar(currentVal, maxVal);
        }

        public void UpdateEnemyChargeBar(int currentVal, int maxVal)
        {
            enemyHUD.SetChargeBar(currentVal, maxVal);
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
                    return "攻击";
                case ActionName.Defense:
                    return "防御";
                case ActionName.Charge:
                    return "蓄能";
                default:
                    return "";
            }
        }

        public void ResetDialogueText()
        {
            dialogueText.text = "选择下一步行动";
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
            bossWinPanel.SetActive(false);
            fakeBossWinPanel.SetActive(false);
        }

        private void OnWinPanelBtnClicked()
        {
            ShowBattleStartUI();
            combatManager.SetUpBattle(false, 1, false);
            HideWinUI();
        }

        private void OnLosePanelBtnClicked()
        {
            ShowBattleStartUI();
            combatManager.SetUpBattle(false, 1, false);
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

        public void ShowBattleStartUI()
        {
            HideButtons();
            HideWinUI();
            HideLoseUI();
            battleStartUI.SetActive(true);
            enemyUnseenUI.SetActive(false);
        }

        private void HideBattleStartUI()
        {
            battleStartUI.SetActive(false);
        }

        public void SetUpPlayerBuffText(string s)
        {
            playerBuffRevealer.SetHoverText(s);
        }

        public void ShowEnemyUnseenUI()
        {
            enemyUnseenUI.SetActive(true);
        }

        public void HideEnemyUnseenUI()
        {
            enemyUnseenUI.SetActive(false);
        }

        public void ShowBossWinUI()
        {
            bossWinPanel.gameObject.SetActive(true);
            HideButtons();
        }

        public void ShowFakeBossWinUI()
        {
            fakeBossWinPanel.gameObject.SetActive(true);
            HideButtons();
        }
    }
}