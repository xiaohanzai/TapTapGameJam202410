using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class EnemyStatsController : CharacterStatsController
    {
        protected override void Start()
        {
            base.Start();
        }

        public ActionName ChooseAction(ActionName playerActionName)
        {
            float p = Random.value;
            switch (playerActionName)
            {
                case ActionName.Attack:
                    if (_currentCharge > 0 && p > 0.5) return ActionName.Attack;
                    else return ActionName.Defense;
                case ActionName.Defense:
                    if (_currentCharge > 0 && p > 0.5) return ActionName.Attack;
                    else return ActionName.Charge;
                case ActionName.Charge:
                    if (_currentCharge > 0 && p > 0.5) return ActionName.Attack;
                    else return ActionName.Charge;
                default:
                    return ActionName.Charge;
            }
        }
    }
}