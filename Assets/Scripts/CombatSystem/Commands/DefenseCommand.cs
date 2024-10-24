using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class DefenseCommand : ICommand
    {
        private CharacterStatsController _character;
        private float _waitTime;

        public DefenseCommand(CharacterStatsController character, float waitTime)
        {
            _character = character;
            _waitTime = waitTime;
        }

        public IEnumerator Co_Execute()
        {
            _character.Defend();
            yield return new WaitForSeconds(_waitTime);
        }
    }
}