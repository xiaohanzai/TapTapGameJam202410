using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class EnemyStatsController : CharacterStatsController
    {
        private int _ind;
        [SerializeField] private EnemyPersonalityType _personalityType;
        private EnemyBuffType _buffType;

        private UIManager uiManager;

        protected override void Start()
        {
            base.Start();

            uiManager = ServiceLocator.Get<UIManager>();

            //Evt_OnCharacterDied.AddListener(() => { gameObject.SetActive(false); });

            Evt_OnCharacterDied.AddListener(uiManager.ShowWinUI);
            Evt_OnChargeChanged.AddListener(uiManager.UpdateEnemyChargeBar);
            Evt_OnHealthChanged.AddListener(uiManager.UpdateEnemyHealthBar);
        }

        public override void SetUp()
        {
            base.SetUp();
            _ind = -1;
        }

        public ActionName ChooseAction(ActionName playerActionName, CombatContext combatContext)
        {
            switch (_personalityType)
            {
                case EnemyPersonalityType.PurelyRandom:
                    return ChooseActionPurelyRandom();
                case EnemyPersonalityType.MindlessCAD:
                    return ChooseActionMindlessCAD();
                case EnemyPersonalityType.MindlessCDA:
                    return ChooseActionMindlessCDA();
                case EnemyPersonalityType.Revenge:
                    return ChooseActionMindlessRevenge(combatContext);
                default:
                    return ChooseActionDefault(playerActionName);
            }
        }

        private ActionName ChooseActionMindlessCAD()
        {
            _ind++;
            switch (_ind % 3)
            {
                case 0:
                    return ActionName.Charge;
                case 1:
                    return ActionName.Attack;
                case 2:
                    return ActionName.Defense;
                default:
                    return ActionName.Charge;
            }
        }

        private ActionName ChooseActionMindlessCDA()
        {
            _ind++;
            switch (_ind % 3)
            {
                case 0:
                    return ActionName.Charge;
                case 1:
                    return ActionName.Defense;
                case 2:
                    return ActionName.Attack;
                default:
                    return ActionName.Charge;
            }
        }

        private ActionName ChooseActionMindlessRevenge(CombatContext combatContext)
        {
            float p = Random.Range(0, 1f);
            if (_currentCharge == 0)
            {
                if (p > 0.1f) return ActionName.Charge;
                else return ActionName.Defense;
            }
            else
            {
                if (combatContext.PlayerActions.Count > 0 && combatContext.PlayerActions[combatContext.PlayerActions.Count - 1] == ActionName.Attack)
                {
                    if (p > 0.1f) return ActionName.Attack;
                    else return ActionName.Defense;
                }
                else
                {
                    if (p > 0.3f) return ActionName.Defense;
                    else if (p < 0.1f) return ActionName.Charge;
                    else return ActionName.Attack;
                }
            }
        }

        private ActionName ChooseActionPurelyRandom()
        {
            float p = Random.Range(0, 1f);
            if (_currentCharge == 0)
            {
                if (p > 0.5f) return ActionName.Charge;
                else return ActionName.Defense;
            }
            else
            {
                if (p < 0.3f) return ActionName.Attack;
                else if (p > 0.6f) return ActionName.Defense;
                else return ActionName.Charge;
            }
        }

        private ActionName ChooseActionDefault(ActionName playerActionName)
        {
            float p = Random.Range(0, 1f);
            switch (playerActionName)
            {
                case ActionName.Attack:
                    if (_currentCharge > 0 && p > 0.5f) return ActionName.Attack;
                    else if (p <= 0.5f) return ActionName.Defense;
                    else return ActionName.Charge;
                case ActionName.Defense:
                    if (_currentCharge > 0 && p > 0.5f) return ActionName.Attack;
                    else return ActionName.Charge;
                case ActionName.Charge:
                    if (_currentCharge > 0 && p > 0.5f) return ActionName.Attack;
                    else return ActionName.Charge;
                default:
                    return ActionName.Charge;
            }
        }
    }
}