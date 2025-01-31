using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class EnemyStatsController : CharacterStatsController
    {
        private EnemyPersonalityType _personalityType;

        private int _ind;

        private UIManager uiManager;

        protected override void Start()
        {
            base.Start();

            uiManager = ServiceLocator.Get<UIManager>();

            //Evt_OnCharacterDied.AddListener(() => { gameObject.SetActive(false); });

            Evt_OnChargeChanged.AddListener(uiManager.UpdateEnemyChargeBar);
            Evt_OnHealthChanged.AddListener(uiManager.UpdateEnemyHealthBar);
        }

        public override void SetUp()
        {
            base.SetUp();

            _currentHealth = _maxHealth;
            Evt_OnHealthChanged.Invoke(_currentHealth, _maxHealth);

            _currentCharge = 0;
            Evt_OnChargeChanged.Invoke(_currentCharge, _maxCharge);

            _ind = -1;

            if (_characterParamsData is EnemyParamsDataSO enemyParamsData)
            {
                _personalityType = enemyParamsData.PersonalityType;
                _buffType = enemyParamsData.BuffType;
            }
            else
            {
                Debug.LogError("Need to input EnemyParamsDataSO for enemies!");
            }
        }

        public void SetUp(float fac)
        {
            SetUp();

            _attackPower *= fac;
        }

        public float GetLightAmount()
        {
            return _maxHealth;
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
                case EnemyPersonalityType.Boss:
                    return ChooseActionBoss(playerActionName);
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

        private ActionName ChooseActionBoss(ActionName playerActionName)
        {
            _ind = (_ind + 1) % 2;
            if (_ind == 0)
            {
                if (playerActionName == ActionName.Attack) return ActionName.Charge;
                else if (playerActionName == ActionName.Defense)
                {
                    if (_currentCharge > 0) return ActionName.Attack;
                    else return ActionName.Defense;
                }
                else return ActionName.Defense;
            }
            else
            {
                if (playerActionName == ActionName.Attack) return ActionName.Defense;
                else if (playerActionName == ActionName.Defense) return ActionName.Charge;
                else
                {
                    if (_currentCharge > 0) return ActionName.Attack;
                    else return ActionName.Charge;
                }
            }
        }

        public string GetDescription()
        {
            switch (_personalityType)
            {
                case EnemyPersonalityType.MindlessCAD:
                    return "循环 蓄力 -> 攻击 -> 防御";
                case EnemyPersonalityType.MindlessCDA:
                    return "循环 蓄力 -> 防御 -> 攻击";
                case EnemyPersonalityType.Revenge:
                    return "如果你进攻，下一轮进攻你";
                case EnemyPersonalityType.PurelyRandom:
                    return "行动完全随机";
                case EnemyPersonalityType.Boss:
                    return "不能告诉你";
                default:
                    return "";
            }
        }
    }
}