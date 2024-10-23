using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class UIDialogueCommand : ICommand
    {
        private string _message;
        private UIManager _uiManager;
        private float _waitTime;

        public UIDialogueCommand(string message, UIManager uiManager, float waitTime)
        {
            _message = message;
            _uiManager = uiManager;
            _waitTime = waitTime;
        }

        public IEnumerator Co_Execute()
        {
            _uiManager.ShowDialogueMessage(_message); // Display dialogue on UI
            yield return new WaitForSeconds(_waitTime); // Wait before proceeding to the next command
        }
    }
}
