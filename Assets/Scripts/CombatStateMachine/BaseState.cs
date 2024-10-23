using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public abstract class BaseState
    {
        protected StateController _controller;                                                                         
               
        public BaseState(StateController controller)
        {      
            _controller = controller;
        }      
               
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void RunState();
        public abstract void ExecuteAction(StateController otherController);
    }
}