using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class StatsResetCommand : ICommand
    {
        CharacterStatsController _controller;

        public StatsResetCommand(CharacterStatsController controller)
        {
            _controller = controller;
        }

        public IEnumerator Co_Execute()
        {
            _controller.ResetStats();
            yield return null;
        }
    }
}