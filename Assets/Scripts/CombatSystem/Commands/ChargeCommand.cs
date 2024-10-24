using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class ChargeCommand : ICommand
    {
        private CharacterStatsController _character;
        private float _waitTime;

        public ChargeCommand(CharacterStatsController character, float waitTime)
        {
            _character = character;
            _waitTime = waitTime;
        }

        public IEnumerator Co_Execute()
        {
            _character.Charge();
            yield return new WaitForSeconds(_waitTime);
        }
    }
}