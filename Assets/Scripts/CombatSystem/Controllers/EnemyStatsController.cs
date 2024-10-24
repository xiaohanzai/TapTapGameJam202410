using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class EnemyStatsController : CharacterStatsController
    {
        private int _ind;
        [SerializeField] private EnemyPersonalityType _personalityType;

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
                    return ChooseActionPurelyRandom(combatContext);
                case EnemyPersonalityType.MindlessCAD:
                    return ChooseActionMindlessCAD();
                case EnemyPersonalityType.MindlessCDA:
                    return ChooseActionMindlessCDA();
                case EnemyPersonalityType.Revenge:
                    return ChooseActionMindlessRevenge(combatContext);
                default:
                    return ChooseActionDefault(playerActionName, combatContext);
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

        private bool DetermineCanDefend(CombatContext combatContext)
        {
            if (_buffType == BuffType.GainChargeFromDefense)
            {
                if (combatContext.EnemyActions.Count > 0 && combatContext.EnemyActions[combatContext.EnemyActions.Count - 1] == ActionName.Defense)
                {
                    return false;
                }
            }
            return true;
        }

        private ActionName ChooseActionMindlessRevenge(CombatContext combatContext)
        {
            float p = Random.Range(0, 1f);
            bool canDefend = DetermineCanDefend(combatContext);
            if (_currentCharge == 0)
            {
                if (p > 0.1f || !canDefend) return ActionName.Charge;
                else return ActionName.Defense;
            }
            else
            {
                if (combatContext.PlayerActions.Count > 0 && combatContext.PlayerActions[combatContext.PlayerActions.Count - 1] == ActionName.Attack)
                {
                    if (p > 0.1f || !canDefend) return ActionName.Attack;
                    else return ActionName.Defense;
                }
                else
                {
                    if (canDefend)
                    {
                        if (p > 0.5f) return ActionName.Defense;
                        else if (p < 0.1f) return ActionName.Charge;
                        else return ActionName.Attack;
                    }
                    else
                    {
                        if (p < 0.7f) return ActionName.Charge;
                        else return ActionName.Attack;
                    }
                }
            }
        }

        private ActionName ChooseActionPurelyRandom(CombatContext combatContext)
        {
            float p = Random.Range(0, 1f);
            bool canDefend = DetermineCanDefend(combatContext);
            if (_currentCharge == 0)
            {
                if (p > 0.5f || !canDefend) return ActionName.Charge;
                else return ActionName.Defense;
            }
            else
            {
                if (canDefend)
                {
                    if (p > 0.6f) return ActionName.Defense;
                    else if (p < 0.3f) return ActionName.Charge;
                    else return ActionName.Attack;
                }
                else
                {
                    if (p < 0.5f) return ActionName.Charge;
                    else return ActionName.Attack;
                }
            }
        }

        private ActionName ChooseActionDefault(ActionName playerActionName, CombatContext combatContext)
        {
            float p = Random.Range(0, 1f);
            bool canDefend = DetermineCanDefend(combatContext);
            switch (playerActionName)
            {
                case ActionName.Attack:
                    if (canDefend)
                    {
                        if (_currentCharge > 0 && p > 0.5f) return ActionName.Attack;
                        else if (p <= 0.5f) return ActionName.Defense;
                        else return ActionName.Charge;
                    }
                    else
                    {
                        if (_currentCharge > 0 && p > 0.5f) return ActionName.Attack;
                        else return ActionName.Charge;
                    }
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