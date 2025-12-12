

using System;
using UnityEngine;

namespace Bakery
{

    public abstract class Condition : ScriptableObject
    {
        [SerializeField] private float _delay;
        private string _timerId = string.Empty;
        public abstract bool Check { get; }
        public bool CanReverse = false;

        public bool CheckTimer()
        {
            if (_timerId == string.Empty)
            {
                _timerId = Quests.Timers().StartNewTimer(_delay);
                return false;
            }
            return !Quests.Timers().IsTimerRunning(_timerId);
        }
    }
}