using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;


namespace Bakery
{
    public class QuestTimerManager : MonoBehaviour, IQuestTimerManager
    {
        [Serializable]
        public class TimerData
        {
            public string id;
            public float duration;
            public float timeLeft;
            public bool isPaused;
            public Action Callback;
        }
        private static readonly List<TimerData> _timers = new();
        void OnEnable()
        {
            Quests.Timers = () => this;
        }
        void OnDisable()
        {
            Quests.Timers = Quests.UnregisterTimer;
        }

        public string StartNewTimer(float duration, Action OnTimerEnd)
        {

            TimerData newTimer = new()
            {
                id = Guid.NewGuid().ToString(),
                duration = duration,
                timeLeft = duration,
                isPaused = false,
                Callback = OnTimerEnd
            };
            _timers.Add(newTimer);
            return newTimer.id;
        }

        public float GetTimeLeft(string timerId)
        {
            TimerData timer = _timers.Find(t => t.id == timerId);

            return (timer == null) ? 0f : timer.timeLeft;

        }

        public void CancelTimer(string timerId)
        {
            _timers.RemoveAll(t => t.id == timerId);
        }

        public void PauseTimer(string timerId)
        {
            TimerData timer = _timers.Find(t => t.id == timerId);
            if (timer != null)
                timer.isPaused = true;
        }

        public void ResumeTimer(string timerId)
        {
            TimerData timer = _timers.Find(t => t.id == timerId);
            if (timer != null)
                timer.isPaused = false;
        }

        public void RestartTimer(string timerId)
        {
            TimerData timer = _timers.Find(t => t.id == timerId);
            if (timer != null)
                timer.timeLeft = timer.duration;
        }

        void Update()
        {
            int i = 0;
            while (i < _timers.Count)
            {
                if (_timers[i].isPaused)
                {
                    i++;
                    continue;
                }
                _timers[i].timeLeft -= Time.deltaTime;
                if (_timers[i].timeLeft <= 0f)
                {
                    _timers[i].Callback?.Invoke();
                    _timers.RemoveAt(i);
                }
                else
                    i++;
            }
        }

        public bool IsTimerRunning(string timerId)
        {
            return _timers.Exists(t => t.id == timerId);
        }
    }


}