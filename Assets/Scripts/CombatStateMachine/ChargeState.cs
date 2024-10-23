using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class ChargeState : BaseState
    {
        public ChargeState(StateController controller) : base(controller)
        {

        }

        public override void EnterState()
        {
            Debug.Log(_controller.gameObject.name + " charging...");
        }

        public override void ExitState()
        {
        }

        public override void RunState()
        {
        }

        public override void ExecuteAction(StateController otherController)
        {
            _controller.Animator.SetTrigger("ChargeTrigger");
            _controller.GetComponent<ChargeController>().AddCharge();
            _controller.CallOnAnimationEnd("Charge", () => {
                Debug.Log(_controller.gameObject.name + " charged");
            });
        }
    }
}