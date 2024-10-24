using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class AnimationCommand : ICommand
    {
        private Animator _animator;
        private string _triggerName;
        private float _waitTime;

        public AnimationCommand(Animator animator, string triggerName, float waitTime)
        {
            _animator = animator;
            _triggerName = triggerName;
            _waitTime = waitTime;
        }

        public IEnumerator Co_Execute()
        {
            _animator.SetTrigger(_triggerName);
            yield return new WaitForSeconds(_waitTime);
        }
    }
}