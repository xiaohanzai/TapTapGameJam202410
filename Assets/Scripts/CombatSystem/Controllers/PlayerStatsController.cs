using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class PlayerStatsController : CharacterStatsController
    {
        private UIManager uiManager;

        private int _indBuff;
        private BuffType[] _availableBuffs = new BuffType[5] { BuffType.DamageIncreaseByCharge, BuffType.TakeDamageOnCharge, BuffType.Shield, BuffType.StartWithOneCharge, BuffType.GainChargeFromDefense };

        protected override void Start()
        {
            base.Start();

            uiManager = ServiceLocator.Get<UIManager>();

            Evt_OnCharacterDied.AddListener(uiManager.ShowLoseUI);
            Evt_OnChargeChanged.AddListener(uiManager.UpdateAttackButtonStatus);
            Evt_OnChargeChanged.AddListener(uiManager.UpdatePlayerChargeBar);
            Evt_OnHealthChanged.AddListener(uiManager.UpdatePlayerHealthBar);
            Evt_OnDefended.AddListener(uiManager.UpdateDefenseButtonStatus);

            uiManager.WinPanel.Evt_OnGetLightClicked.AddListener(AddLight);
            uiManager.WinPanel.Evt_OnGetBuffClicked.AddListener(SetBuffType);
        }

        public override void SetUp()
        {
            base.SetUp();

            _currentHealth = _maxHealth;
            Evt_OnHealthChanged.Invoke(_currentHealth, _maxHealth);

            _currentCharge = 0;
            Evt_OnChargeChanged.Invoke(_currentCharge, _maxCharge);

            if (_buffType == BuffType.Null) _buffType = _availableBuffs[Random.Range(0, _availableBuffs.Length)];
        }

        public void SetUp(float health, int charge, float fac)
        {
            base.SetUp();

            _attackPower /= fac;

            _currentHealth = health > 0? health : _maxHealth;
            Evt_OnHealthChanged.Invoke(_currentHealth, _maxHealth);

            _currentCharge = charge > 0? charge : 0;
            Evt_OnChargeChanged.Invoke(_currentCharge, _maxCharge);

            // for debugging
            //_buffType = _availableBuffs[_indBuff % 5];
            //_indBuff++;
            if (_buffType == BuffType.Null) _buffType = _availableBuffs[Random.Range(0, _availableBuffs.Length)];
        }

        public void AddLight(float amount)
        {
            _currentHealth += amount;
            if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
            Evt_OnHealthChanged.Invoke(_currentHealth, _maxHealth);
        }

        public void LoseLight(float amount)
        {
            _currentHealth -= amount;
            if (_currentHealth < 0) _currentHealth = 0;
            Evt_OnHealthChanged.Invoke(_currentHealth, _maxHealth);
        }

    }
}