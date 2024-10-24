using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class PlayerStatsController : CharacterStatsController
    {
        private UIManager uiManager;

        protected override void Start()
        {
            base.Start();

            uiManager = ServiceLocator.Get<UIManager>();

            Evt_OnCharacterDied.AddListener(uiManager.ShowLoseUI);
            Evt_OnChargeChanged.AddListener(uiManager.UpdateAttackButtonStatus);
            Evt_OnChargeChanged.AddListener(uiManager.UpdatePlayerChargeBar);
            Evt_OnHealthChanged.AddListener(uiManager.UpdatePlayerHealthBar);
        }

        public override void SetUp()
        {
            base.SetUp();
        }
    }
}