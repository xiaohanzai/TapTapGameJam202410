using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class DefenseState : BaseState
    {
        public DefenseState(StateController controller) : base(controller)
        {

        }

        public override void EnterState()
        {
            Debug.Log(_controller.gameObject.name + " defensing");
        }

        public override void ExitState()
        {
        }

        public override void RunState()
        {
        }

        public override void ExecuteAction(StateController otherController)
        {
            _controller.Animator.SetTrigger("DefenseTrigger");
            _controller.CallOnAnimationEnd("Defense", () => {
                Debug.Log(_controller.gameObject.name + " defended");
            });
        }
    }
}