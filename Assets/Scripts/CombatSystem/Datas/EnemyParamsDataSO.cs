using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = "NewEnemyParamsData", menuName = "SOs/Data/Enemy Params")]
    public class EnemyParamsDataSO : CharacterParamsDataSO
    {
        [SerializeField] private int _lightAmount;
        public int LightAmount => _lightAmount;

        [SerializeField] private BuffType _buffType;
        public BuffType BuffType => _buffType;

        [SerializeField] private EnemyPersonalityType _personalityType;
        public EnemyPersonalityType PersonalityType => _personalityType;
    }
}
