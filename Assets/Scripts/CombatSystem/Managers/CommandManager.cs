using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class CommandManager : MonoBehaviour
    {
        private Queue<ICommand> _commandQueue = new Queue<ICommand>();

        public void AddCommand(ICommand command)
        {
            _commandQueue.Enqueue(command);
        }

        public void ExecuteCommands()
        {
            StartCoroutine(ExecuteCommandSequence());
        }

        private IEnumerator ExecuteCommandSequence()
        {
            while (_commandQueue.Count > 0)
            {
                ICommand command = _commandQueue.Dequeue();
                yield return StartCoroutine(command.Co_Execute());
            }
        }
    }
}