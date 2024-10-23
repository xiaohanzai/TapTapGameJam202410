using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class AttackCommand : ICommand
    {
        private CharacterStatsController _attacker;
        private CharacterStatsController _target;
        private float _waitTime;

        public AttackCommand(CharacterStatsController attacker, CharacterStatsController target, float waitTime)
        {
            _attacker = attacker;
            _target = target;
            _waitTime = waitTime;
        }

        public IEnumerator Co_Execute()
        {
            _target.TakeDamage(_attacker.AttackPower);
            _attacker.LoseCharge();
            yield return new WaitForSeconds(_waitTime);
        }
    }
}