using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class IdleState : BaseState
    {
        public IdleState(StateController controller) : base(controller)
        {
        }

        public override void EnterState()
        {
            Debug.Log(_controller.gameObject.name + " idling...");
        }

        public override void ExitState()
        {
        }

        public override void RunState()
        {
        }

        public override void ExecuteAction(StateController otherController)
        {
        }
    }
}