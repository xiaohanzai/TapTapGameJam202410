using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public interface ICommand
    {
        //void Execute();
        IEnumerator Co_Execute();
    }
}