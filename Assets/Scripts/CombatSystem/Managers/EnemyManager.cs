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

            foreach (var item in enemyStatsControllers)
            {
                item.Evt_OnCharacterDied.AddListener(OnCurrentEnemyDied);
            }
        }

        private void OnCurrentEnemyDied()
        {
            ind++;
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
    }
}