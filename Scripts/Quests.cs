
using System;
using UnityEngine;


namespace Bakery
{
    public static class Quests
    {
        public static class Events
        {
            public static Action<QuestData> OnQuestStarted = delegate { };
            public static Action<QuestData> OnQuestInterrupted = delegate { };
            public static Action<Quest, Condition> OnConditionCompleted = delegate { };
            public static Action<Quest, QuestData.Step> OnStepCompleted = delegate { };
            public static Action<Quest> OnQuestCompleted = delegate { };
        }

        public static Func<IQuestManager> Manager = UnregisterManager;
        public static Func<IQuestTimerManager> Timers = UnregisterTimer;

        private static IQuestManager _cachedManager = null;
        private static IQuestTimerManager _cachedTimer = null;

        public static IQuestManager UnregisterManager()
        {
            Debug.LogWarning("[QuestServices] No Quest Manager registered. Please register a Quest Manager before using Quest Services.");
            _cachedManager ??= new MockQuestManager();
            Manager = () => _cachedManager;
            return _cachedManager;
        }

        public static IQuestTimerManager UnregisterTimer()
        {
            Debug.LogWarning("[QuestServices] No Quest Timer registered. Please register a Quest Timer before using Quest Services.");
            _cachedTimer ??= new MockQuestTimer();
            Timers = () => _cachedTimer;
            return _cachedTimer;
        }

        private class MockQuestTimer : IQuestTimerManager
        {
            public void CancelTimer(string timerId)
            { }

            public float GetTimeLeft(string timerId)
            { return 0f; }

            public bool IsTimerRunning(string timerId)
            {
                return false;
            }

            public void PauseTimer(string timerId)
            { }

            public void RestartTimer(string timerId)
            { }

            public void ResumeTimer(string timerId)
            { }

            public string StartNewTimer(float duration, Action OnTimerEnd)
            { return string.Empty; }
        }
        private class MockQuestManager : IQuestManager
        {
            public WaitUntil WaitUntilReady => new(() => true);

            public void ForceConditionCheck(QuestData data)
            {
            }

            public void ForceComplete(QuestData data)
            {
            }
            public void ForceComplete(Quest quest)
            {
            }

            public bool IsQuestCompleted(QuestData data)
            {
                return false;
            }

            public QuestData.Step GetCurrentStep(QuestData data)
            {
                return null;
            }

            public void InterruptQuest(QuestData data)
            {
            }

            public void StartQuest(QuestData data)
            {
            }

            public void StartQuestByName(string questName)
            {
            }
        }

        //Cleaning stuff in case cowboys are fast reloading in the editor
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            Events.OnConditionCompleted = delegate { };
            Events.OnQuestCompleted = delegate { };
            Events.OnQuestInterrupted = delegate { };
            Events.OnQuestStarted = delegate { };
            Events.OnStepCompleted = delegate { };

            Manager = UnregisterManager;

#if UNITY_EDITOR
            Debug.Log("[Quest] Static fields reset (domain reload skipped)");
#endif
        }

    }


}