using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemyStatsController[] enemyStatsControllers;

        private int ind;

        private UIManager uiManager;

        void Start()
        {
            uiManager = ServiceLocator.Get<UIManager>();

            for (int i = 0; i < enemyStatsControllers.Length; i++)
            {
                if (i < enemyStatsControllers.Length - 2)
                {
                    enemyStatsControllers[i].Evt_OnCharacterDied.AddListener(uiManager.ShowWinUI);
                    enemyStatsControllers[i].Evt_OnCharacterDied.AddListener(OnCurrentEnemyDied);
                }
                else if (i == enemyStatsControllers.Length - 2) enemyStatsControllers[i].Evt_OnCharacterDied.AddListener(uiManager.ShowFakeBossWinUI);
                else enemyStatsControllers[i].Evt_OnCharacterDied.AddListener(uiManager.ShowBossWinUI);
            }
        }

        private void OnCurrentEnemyDied()
        {
            ind = (ind + 1) % (enemyStatsControllers.Length - 1);
        }

        public EnemyStatsController GetAndShowCurrentEnemy()
        {
            for (int i = 0; i < enemyStatsControllers.Length; i++)
            {
                if (i == ind) enemyStatsControllers[i].gameObject.SetActive(true);
                else enemyStatsControllers[i].gameObject.SetActive(false);
            }
            return ind < enemyStatsControllers.Length? enemyStatsControllers[ind] : null;
        }

        public void SetEnemy(int i)
        {
            ind = i % (enemyStatsControllers.Length - 2);
            if (i < 0) ind = enemyStatsControllers.Length + i;
        }
    }
}