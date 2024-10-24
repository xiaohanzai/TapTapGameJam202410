using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class ServiceLocatorInitManager : MonoBehaviour
    {
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private CombatManager combatManager;
        [SerializeField] private CommandManager commandManager;

        private void Awake()
        {
            // Register all services at the start
            ServiceLocator.Register(enemyManager);
            ServiceLocator.Register(uiManager);
            ServiceLocator.Register(combatManager);
            ServiceLocator.Register(commandManager);
        }

        private void OnDestroy()
        {
            // Unregister services on destroy (optional)
            ServiceLocator.Unregister<EnemyManager>();
            ServiceLocator.Unregister<UIManager>();
            ServiceLocator.Unregister<CombatManager>();
            ServiceLocator.Unregister<CommandManager>();
        }
    }
}