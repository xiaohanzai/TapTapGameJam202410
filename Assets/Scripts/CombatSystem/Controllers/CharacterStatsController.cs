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
        public UnityEvent<int, int> Evt_OnHealthChanged = new UnityEvent<int, int>();
        public UnityEvent<bool> Evt_OnDefended = new UnityEvent<bool>();
        public UnityEvent Evt_OnCharacterDied = new UnityEvent();

        protected int _currentHealth;
        protected int _maxHealth;

        protected int _currentCharge;
        protected int _maxCharge;
        protected int _thisRoundChargeLoss;

        protected int _attackPower;
        protected int _thisRoundAttackPower;

        protected int _lightAmount;

        private int _shield;

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
                    return "start with one charge.";
                case BuffType.DamageIncreaseByCharge:
                    return shortVersion ? "more charge, higher attack" : "more attack power with more charge, at the cost of losing all charge.";
                case BuffType.Shield:
                    _shield = _maxHealth / 3;
                    return shortVersion ? "shield." : "can shield some damage taken.";
                case BuffType.TakeDamageOnCharge:
                    return shortVersion ? "take damage on charge." : "can convert health damage to losing charge.";
                case BuffType.GainChargeFromDefense:
                    return shortVersion ? "charge on defense." : "gain charge from defending, but cannot defend consecutively.";
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

        public void TakeDamage(int damage)
        {
            if (_isDefending) return;

            if (_buffType == BuffType.TakeDamageOnCharge && _currentCharge > _thisRoundChargeLoss)
            {
                int damageOnCharge = Mathf.Min(damage, _currentCharge - _thisRoundChargeLoss);
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

        public int GetAttackPower()
        {
            return _thisRoundAttackPower;
        }

        public void RevealCombatResults()
        {
            Evt_OnChargeChanged.Invoke(_currentCharge, _maxCharge);
            Evt_OnHealthChanged.Invoke(Mathf.Max(_currentHealth, 0), _maxHealth);
            if (_currentHealth <= 0) Evt_OnCharacterDied.Invoke();
        }

        public void AddLight(int amount)
        {
            _lightAmount += amount;
        }

        public void LoseLight(int amount)
        {
            _lightAmount -= amount;
        }

        public int GetLightAmount()
        {
            return _lightAmount;
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