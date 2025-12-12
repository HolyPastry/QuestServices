
using System;


namespace Bakery
{
    public interface IQuestTimerManager
    {
        bool IsTimerRunning(string timerId);
        string StartNewTimer(float duration, Action OnTimerEnd = null);
        float GetTimeLeft(string timerId);
        void CancelTimer(string timerId);
        void PauseTimer(string timerId);
        void ResumeTimer(string timerId);
        void RestartTimer(string timerId);
    }


}