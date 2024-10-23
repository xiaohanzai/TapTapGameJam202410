using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class AttackState : BaseState
    {
        public AttackState(StateController controller) : base(controller)
        {

        }

        public override void EnterState()
        {
            Debug.Log(_controller.gameObject.name + " attacking");
        }

        public override void ExitState()
        {
        }

        public override void RunState()
        {
        }

        public override void ExecuteAction(StateController otherController)
        {
            _controller.Animator.SetTrigger("AttackTrigger");
            if (!(otherController.GetCurrentState() is DefenseState))
            {
                _controller.CallOnAnimationEnd("Attack", () => {
                    otherController.GetComponent<HealthController>().LoseHealth();
                    Debug.Log(_controller.gameObject.name + " attacked");
                });
            }
            _controller.GetComponent<ChargeController>().LoseCharge();
        }
    }
}