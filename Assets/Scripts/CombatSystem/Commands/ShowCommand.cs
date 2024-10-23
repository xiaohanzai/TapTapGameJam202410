using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class ShowCommand : ICommand
    {
        private GameObject _go;
        private float _waitTime;

        public ShowCommand(GameObject go, float waitTime)
        {
            _go = go;
            _waitTime = waitTime;
        }

        public IEnumerator Co_Execute()
        {
            _go.SetActive(true);
            yield return new WaitForSeconds(_waitTime);
        }
    }
}