using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem;

public class EnemyStateController : StateController
{
    protected override void Start()
    {
        base.Start();
    }

    public StateName DecideNextMove(StateName playerStateName)
    {
        float p = Random.value;
        int charge = GetComponent<ChargeController>().GetCurrentCharge();
        switch (playerStateName)
        {
            case StateName.Attack:
                if (charge > 0 && p > 0.5) return StateName.Attack;
                else return StateName.Defense;
            case StateName.Charge:
                if (charge > 0 && p > 0.5f) return StateName.Attack;
                else return StateName.Charge;
            case StateName.Defense:
                if (charge > 0 && p > 0.5f) return StateName.Attack;
                else if (charge == 0) return StateName.Charge;
                else return StateName.Defense;
            default:
                return StateName.Charge;
        }
    }
}
