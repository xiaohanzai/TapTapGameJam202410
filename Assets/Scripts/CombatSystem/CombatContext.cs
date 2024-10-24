using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class CombatContext
    {
        private List<ActionName> _playerActions;
        public List<ActionName> PlayerActions => _playerActions;
        private List<ActionName> _enemyActions;
        public List<ActionName> EnemyActions => _enemyActions;

        public CombatContext()
        {
            _playerActions = new List<ActionName>();
            _enemyActions = new List<ActionName>();
        }

        public void AddToPlayerActions(ActionName actionName)
        {
            _playerActions.Add(actionName);
        }

        public void AddToEnemyActions(ActionName actionName)
        {
            _enemyActions.Add(actionName);
        }

        public void ClearPlayerActions()
        {
            _playerActions.Clear();
        }

        public void ClearEnemyActions()
        {
            _enemyActions.Clear();
        }
    }
}