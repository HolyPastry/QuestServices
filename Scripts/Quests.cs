
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

        private static IQuestManager _cachedManager = null;

        public static IQuestManager UnregisterManager()
        {
            Debug.LogWarning("[QuestServices] No Quest Manager registered. Please register a Quest Manager before using Quest Services.");
            _cachedManager ??= new MockQuestManager();
            Manager = () => _cachedManager;
            return _cachedManager;
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


    }


}