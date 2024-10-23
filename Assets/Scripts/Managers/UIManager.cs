using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button chargeButton;
    [SerializeField] private Button defenseButton;

    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private ChargeController playerChargeController;

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
        attackButton.onClick.AddListener(OnPlayerChooseAttack);
        chargeButton.onClick.AddListener(OnPlayerChooseCharge);
        defenseButton.onClick.AddListener(OnPlayerChooseDefense);

        playerChargeController.Evt_OnChargeChanged.AddListener(UpdateAttackButtonStatus);
    }

    private void OnDestroy()
    {
        attackButton.onClick.RemoveListener(OnPlayerChooseAttack);
        chargeButton.onClick.RemoveListener(OnPlayerChooseCharge);
        defenseButton.onClick.RemoveListener(OnPlayerChooseDefense);

        playerChargeController.Evt_OnChargeChanged.RemoveListener(UpdateAttackButtonStatus);
    }

    void Update()
    {
        
    }

    private void UpdateAttackButtonStatus(bool enabled)
    {
        attackButton.enabled = enabled;
    }

    private void OnPlayerChooseAttack()
    {
        CombatManager.Instance.OnPlayerActionChosen(CombatSystem.StateName.Attack);
    }

    private void OnPlayerChooseDefense()
    {
        CombatManager.Instance.OnPlayerActionChosen(CombatSystem.StateName.Defense);
    }

    private void OnPlayerChooseCharge()
    {
        CombatManager.Instance.OnPlayerActionChosen(CombatSystem.StateName.Charge);
    }

    public void ShowButtons()
    {
        buttonsParent.SetActive(true);
    }

    public void HideButtons()
    {
        buttonsParent.SetActive(false);
    }

    private string GetStateText(CombatSystem.StateName stateName)
    {
        switch (stateName)
        {
            case CombatSystem.StateName.Attack:
                return "attack";
            case CombatSystem.StateName.Charge:
                return "charge";
            case CombatSystem.StateName.Defense:
                return "defend";
            default:
                return "";
        }
    }

    public void UpdateDialogueText(CombatSystem.StateName playerState, CombatSystem.StateName enemyState)
    {
        dialogueText.text = "Player chooses to " + GetStateText(playerState) + "\n" + "Enemy chooses to " + GetStateText(enemyState);
    }

    public void ResetDialogueText()
    {
        dialogueText.text = "Choose an action";
    }
}
