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

        protected int _currentHealth;
        protected int _maxHealth;

        protected int _currentCharge;
        protected int _maxCharge;

        protected int _attackPower;
        public int AttackPower => _attackPower;

        protected float _attackAnimationDuration;
        public float AttackAnimationDuration => _attackAnimationDuration;
        protected float _chargeAnimationDuration;
        public float ChargeAnimationDuration => _chargeAnimationDuration;
        protected float _defenseAnimationDuration;
        public float DefenseAnimationDuration => _defenseAnimationDuration;

        protected bool _isDefending;

        protected virtual void Start()
        {
            _maxHealth = _characterParamsData.MaxHealth;
            _currentHealth = _maxHealth;
            Evt_OnHealthChanged.Invoke(_currentHealth, _maxHealth);

            _maxCharge = _characterParamsData.MaxCharge;
            _currentCharge = 0;
            Evt_OnChargeChanged.Invoke(_currentCharge, _maxCharge);

            _attackPower = _characterParamsData.AttackPower;

            _attackAnimationDuration = AnimationHelper.GetAnimationLength(_animator, "Attack");
            _chargeAnimationDuration = AnimationHelper.GetAnimationLength(_animator, "Charge");
            _defenseAnimationDuration = AnimationHelper.GetAnimationLength(_animator, "Defense");
        }

        public void QueueInActionCommand(ActionName actionName, float waitTime, CharacterStatsController otherController, CommandManager commandManager)
        {
            switch (actionName)
            {
                case ActionName.Attack:
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

        public void QueueInAnimationCommand(ActionName actionName, float waitTime, CommandManager commandManager)
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

        public void QueueInResetStatsCommand(CommandManager commandManager)
        {
            commandManager.AddCommand(new StatsResetCommand(this));
        }

        public void TakeDamage(int damage)
        {
            if (!_isDefending) _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
            }
            Evt_OnHealthChanged.Invoke(_currentHealth, _maxHealth);
        }

        public void Defend()
        {
            _isDefending = true;
        }

        public void ResetStats()
        {
            _isDefending = false;
        }

        public void Charge()
        {
            if (_currentCharge < _maxCharge) _currentCharge++;
            Evt_OnChargeChanged.Invoke(_currentCharge, _maxCharge);
        }

        public void LoseCharge()
        {
            if (_currentCharge > 0) _currentCharge--;
            Evt_OnChargeChanged.Invoke(_currentCharge, _maxCharge);
        }
    }
}