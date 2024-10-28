using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem
{
    public class CharacterStatsController : MonoBehaviour
    {
        [SerializeField] protected CharacterParamsDataSO _characterParamsData;

        [SerializeField] protected Animator _animator;

        public UnityEvent<int, int> Evt_OnChargeChanged = new UnityEvent<int, int>();
        public UnityEvent<float, int> Evt_OnHealthChanged = new UnityEvent<float, int>();
        public UnityEvent<bool> Evt_OnDefended = new UnityEvent<bool>();
        public UnityEvent Evt_OnCharacterDied = new UnityEvent();

        protected float _currentHealth;
        protected int _maxHealth;

        protected int _currentCharge;
        protected int _maxCharge;
        protected int _thisRoundChargeLoss;

        protected float _attackPower;
        protected float _thisRoundAttackPower;

        protected float _lightAmount;

        private float _shield;

        protected float _attackAnimationDuration;
        public float AttackAnimationDuration => _attackAnimationDuration;
        protected float _chargeAnimationDuration;
        public float ChargeAnimationDuration => _chargeAnimationDuration;
        protected float _defenseAnimationDuration;
        public float DefenseAnimationDuration => _defenseAnimationDuration;

        protected bool _isDefending;
        protected int _indFromDefense;

        protected CommandManager commandManager;

        protected BuffType _buffType;

        protected virtual void Start()
        {
            commandManager = ServiceLocator.Get<CommandManager>();
        }

        public virtual void SetUp()
        {
            _maxHealth = _characterParamsData.MaxHealth;

            _maxCharge = _characterParamsData.MaxCharge;
            
            _thisRoundChargeLoss = 0;

            _attackPower = _characterParamsData.AttackPower;

            _shield = 0;

            _attackAnimationDuration = AnimationHelper.GetAnimationLength(_animator, "Attack");
            _chargeAnimationDuration = AnimationHelper.GetAnimationLength(_animator, "Charge");
            _defenseAnimationDuration = AnimationHelper.GetAnimationLength(_animator, "Defense");

            _isDefending = false;
            _indFromDefense = -1;
        }

        public bool IsDead()
        {
            return _currentHealth <= 0;
        }

        public string RevealBuff(bool shortVersion)
        {
            switch (_buffType)
            {
                case BuffType.StartWithOneCharge:
                    _currentCharge = 1;
                    Evt_OnChargeChanged.Invoke(_currentCharge, _maxCharge);
                    return "起始蓄能为 1";
                case BuffType.DamageIncreaseByCharge:
                    return shortVersion ? "蓄能越多攻击越高" : "蓄力越多攻击越高，但会消耗所有蓄能";
                case BuffType.Shield:
                    _shield = _maxHealth / 3;
                    return shortVersion ? "盾牌" : "在剩余1/3血量时开始抵挡一部分攻击";
                case BuffType.TakeDamageOnCharge:
                    return shortVersion ? "用蓄能代替血量受攻击" : "如有多余的蓄能，用蓄能代替血量受攻击";
                case BuffType.GainChargeFromDefense:
                    return shortVersion ? "防御时增加蓄能" : "防御时增加蓄能，但不能连续防御";
                default:
                    return "no buff!";
            }
        }

        public void QueueInActionCommand(ActionName actionName, float waitTime, CharacterStatsController otherController)
        {
            switch (actionName)
            {
                case ActionName.Attack:
                    _thisRoundChargeLoss = 1;
                    _thisRoundAttackPower = _attackPower;
                    if (_buffType == BuffType.DamageIncreaseByCharge)
                    {
                        _thisRoundChargeLoss = _currentCharge;
                        _thisRoundAttackPower *= _currentCharge;
                    }
                    //Debug.Log(gameObject.name + " attack power = " + _thisRoundAttackPower);
                    commandManager.AddCommand(new AttackCommand(this, otherController, waitTime));
                    break;
                case ActionName.Charge:
                    commandManager.AddCommand(new ChargeCommand(this, waitTime));
                    break;
                case ActionName.Defense:
                    commandManager.AddCommand(new DefenseCommand(this, waitTime));
                    break;
                default:
                    break;
            }
        }

        public void QueueInAnimationCommand(ActionName actionName, float waitTime)
        {
            switch (actionName)
            {
                case ActionName.Attack:
                    commandManager.AddCommand(new AnimationCommand(_animator, "AttackTrigger", waitTime));
                    break;
                case ActionName.Charge:
                    commandManager.AddCommand(new AnimationCommand(_animator, "ChargeTrigger", waitTime));
                    break;
                case ActionName.Defense:
                    commandManager.AddCommand(new AnimationCommand(_animator, "DefenseTrigger", waitTime));
                    break;
                default:
                    break;
            }
        }

        public void QueueInResetStatsCommand()
        {
            commandManager.AddCommand(new StatsResetCommand(this));
        }

        public void TakeDamage(float damage)
        {
            if (_isDefending) return;

            if (_buffType == BuffType.TakeDamageOnCharge && _currentCharge > _thisRoundChargeLoss)
            {
                int damageOnCharge = Mathf.Min(Mathf.CeilToInt(damage), _currentCharge - _thisRoundChargeLoss);
                _currentCharge -= damageOnCharge;
                damage -= damageOnCharge;
            }
            _currentHealth -= damage;
            if (_shield > 0 && _currentHealth < _shield)
            {
                _currentHealth = _shield;
                _shield--;
            }
        }

        public void Defend()
        {
            _isDefending = true;

            if (_buffType == BuffType.GainChargeFromDefense)
            {
                //float p = Random.Range(0, 1f);
                //if (p > 0.2f)
                //{
                _currentCharge++;
                //}
                _indFromDefense++;
                Evt_OnDefended.Invoke(false);
            }
        }

        public void ResetStats()
        {
            _isDefending = false;
            _thisRoundChargeLoss = 0;
            Evt_OnDefended.Invoke(true);
            _indFromDefense = -1;
        }

        public void Charge()
        {
            if (_currentCharge < _maxCharge) _currentCharge++;
        }

        public void LoseCharge()
        {
            _currentCharge -= _thisRoundChargeLoss;
            if (_currentCharge < 0)
            {
                Debug.Log(gameObject.name + " charge <= 0!  charge = " + _currentCharge);
            }
        }

        public float GetAttackPower()
        {
            return _thisRoundAttackPower;
        }

        public void RevealCombatResults()
        {
            Evt_OnChargeChanged.Invoke(_currentCharge, _maxCharge);
            Evt_OnHealthChanged.Invoke(Mathf.Max(_currentHealth, 0), _maxHealth);
            if (_currentHealth <= 0) Evt_OnCharacterDied.Invoke();
        }

        public void SetBuffType(BuffType buffType)
        {
            _buffType = buffType;
        }

        public BuffType GetBuffType()
        {
            return _buffType;
        }
    }
}