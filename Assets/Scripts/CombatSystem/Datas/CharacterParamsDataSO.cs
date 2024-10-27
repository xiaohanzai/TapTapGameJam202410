using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = "NewCharacterParamsData", menuName = "SOs/Data/Character Params")]
    public class CharacterParamsDataSO : ScriptableObject
    {
        [SerializeField] private int _maxHealth;
        public int MaxHealth => _maxHealth;

        [SerializeField] private int _maxCharge;
        public int MaxCharge => _maxCharge;

        [SerializeField] private float _attackPower;
        public float AttackPower => _attackPower;
    }
}