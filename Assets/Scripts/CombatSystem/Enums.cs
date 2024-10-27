using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public enum ActionName
    {
        Attack,
        Defense,
        Charge,
    }

    public enum EnemyPersonalityType
    {
        MindlessCAD,
        MindlessCDA,
        Revenge,
        PurelyRandom,
        Boss,
    }

    public enum BuffType
    {
        Null,
        StartWithOneCharge,
        DamageIncreaseByCharge,
        Shield,
        TakeDamageOnCharge,
        GainChargeFromDefense,
    }
}