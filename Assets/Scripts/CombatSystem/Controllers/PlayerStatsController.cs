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
        }

        public override void SetUp()
        {
            base.SetUp();

            _buffType = _availableBuffs[_indBuff % 5];
            _indBuff++;
        }
    }
}